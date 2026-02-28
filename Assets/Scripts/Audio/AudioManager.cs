using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Volume")]
    [Range(0, 1)] public float masterVolume = 1;
    [Range(0, 1)] public float musicVolume = 1;
    [Range(0, 1)] public float ambienceVolume = 1;
    [Range(0, 1)] public float SFXVolume = 1;

    [Header("Music Events")]
    public EventReference menuMusic;
    public EventReference gameplayMusic;
    public EventReference desktopScene;


    [Header("Cinematicas")]

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private EventInstance currentMusic;

    private Dictionary<string, EventInstance> cinematicInstances = new Dictionary<string, EventInstance>();
    private Coroutine musicParameterCoroutine;
    private float currentModesValue = 0f;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");

        SetupLowLatency();
        PlayMusic(menuMusic);
    }

    private void SetupLowLatency()
    {
        try
        {
            // Solo configurar desde Unity, no modificar FMOD directamente
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.dspBufferSize = 256; // Valor más bajo = menos latencia
            AudioSettings.Reset(config);

            Debug.Log("Configuración de audio de Unity ajustada para baja latencia");
        }
        catch
        {
            // Silenciosamente continuar si falla
        }
    }

    private void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(SFXVolume);
    }


    #region Musica

    public void PlayMusic(EventReference musicEvent)
    {
        if (!musicEvent.IsNull)
        {
            Debug.Log($"Reproduciendo música");

            if (currentMusic.isValid())
            {
                currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                currentMusic.release();
            }

            currentMusic = RuntimeManager.CreateInstance(musicEvent);
            currentMusic.start();
            currentMusic.setVolume(musicVolume);
        }
        else
        {
            Debug.LogWarning("Intento de reproducir música con EventReference nula");
        }
    }

    public void PlayMusicImmediate(EventReference musicEvent)
    {
        if (!musicEvent.IsNull)
        {
            Debug.Log($"Reproduciendo música inmediata");

            if (currentMusic.isValid())
            {
                currentMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                currentMusic.release();
            }

            currentMusic = RuntimeManager.CreateInstance(musicEvent);
            currentMusic.start();
            currentMusic.setVolume(musicVolume);
        }
    }


    public void ReturnToGameplayImmediate()
    {
        Debug.Log("AudioManager: Volviendo a gameplay inmediatamente (sin fade)");
        PlayMusicImmediate(gameplayMusic);
    }

    public void ReturnToMenuImmediate()
    {
        Debug.Log("AudioManager: Volviendo a menú inmediatamente (sin fade)");
        PlayMusicImmediate(menuMusic);
    }


    public void ChangeMusicWithFade(EventReference newMusic, float fadeTime = 0f)
    {
        StartCoroutine(FadeMusicRoutine(newMusic, fadeTime));
    }

    private IEnumerator FadeMusicRoutine(EventReference newMusic, float fadeTime)
    {
        if (newMusic.IsNull) yield break;

        float timer = 0f;
        float startVolume = musicVolume;

        EventInstance newMusicInstance = RuntimeManager.CreateInstance(newMusic);
        newMusicInstance.start();

        EventInstance oldMusic = currentMusic;
        currentMusic = newMusicInstance;

        while (timer < fadeTime)
        {
            float t = timer / fadeTime;

            if (oldMusic.isValid())
            {
                float oldVolume = Mathf.Lerp(startVolume, 0f, t);
                oldMusic.setVolume(oldVolume);
            }

            float newVolume = Mathf.Lerp(0f, startVolume, t);
            newMusicInstance.setVolume(newVolume);

            timer += Time.deltaTime;
            yield return null;
        }

        if (oldMusic.isValid())
        {
            oldMusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            oldMusic.release();
        }
    }

    public void SetMusicParameterSmooth(string parameterName, float targetValue, float duration = 1f)
    {
        if (!currentMusic.isValid()) return;

        if (musicParameterCoroutine != null)
            StopCoroutine(musicParameterCoroutine);

        musicParameterCoroutine = StartCoroutine(
            FadeMusicParameter(parameterName, targetValue, duration)
        );
    }

    private IEnumerator FadeMusicParameter(string parameterName, float targetValue, float duration)
    {
        if (!currentMusic.isValid()) yield break;

        float startValue = currentModesValue;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            float smoothT = t * t * (3f - 2f * t);
            float value = Mathf.Lerp(startValue, targetValue, smoothT);
            currentMusic.setParameterByName(parameterName, value);

            currentModesValue = value;
            yield return null;
        }

        currentModesValue = targetValue;
        currentMusic.setParameterByName(parameterName, targetValue);
    }

    private IEnumerator FadeInNewMusic(EventReference newMusic, float fadeTime)
    {
        if (newMusic.IsNull)
        {
            Debug.LogWarning("No se puede hacer fade in de música nula");
            yield break;
        }

        if (IsPlayingMusic(newMusic))
        {
            Debug.Log($"AudioManager: Ya está reproduciendo esta música, manteniendo");

            if (currentMusic.isValid())
            {
                currentMusic.setVolume(musicVolume);
            }
            yield break;
        }

        Debug.Log($"Iniciando fade in para nueva música");

        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

        currentMusic = RuntimeManager.CreateInstance(newMusic);
        currentMusic.start();

        currentMusic.setVolume(0f);

        float timer = 0f;
        while (timer < fadeTime)
        {
            float t = timer / fadeTime;

            float targetVolume = Mathf.Lerp(0f, musicVolume, t);

            currentMusic.setVolume(targetVolume);

            timer += Time.deltaTime;
            yield return null;
        }

        currentMusic.setVolume(musicVolume);

        Debug.Log($"Fade in completado con volumen: {musicVolume}");
    }

    #endregion

    #region SFX

    public void PlayOneShot(EventReference sound)
    {
        if (!sound.IsNull)
            RuntimeManager.PlayOneShot(sound);
    }

    #endregion

    #region Cambios de Escena

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return; //He metido yo esto Aimar UwU
        Debug.Log($"AudioManager: Escena cargada - {scene.name}");

        if (scene.name == "SampleScene" && IsPlayingMusic(gameplayMusic))
        {
            Debug.Log("AudioManager: Ya está sonando música de gameplay, manteniendo");
            ApplyShopLowcut(false);
            return;
        }

        if (scene.name == "Menu" && IsPlayingMusic(menuMusic))
        {
            Debug.Log("AudioManager: Ya está sonando música de menú, manteniendo");
            ApplyShopLowcut(false);
            return;
        }

        EventReference newMusic = default;

        switch (scene.name)
        {
            case "RoomScene":
                newMusic = menuMusic;
                Debug.Log("AudioManager: Música de Menú");
                ApplyShopLowcut(false);
                break;

            case "DesktopScene":
                newMusic = desktopScene;
                Debug.Log("AudioManager: Música de Gameplay");
                ApplyShopLowcut(false);
                break;
            case "GameplayScene":
                newMusic = gameplayMusic;
                Debug.Log("AudioManager: Música de Gameplay");
                ApplyShopLowcut(false);
                break;
        }

        if (!newMusic.IsNull)
        {
            Debug.Log($"AudioManager: Cambiando a nueva música");
            StartCoroutine(FadeInNewMusic(newMusic, 0.5f));
        }
    }

    private void ApplyShopLowcut(bool enable)
    {
        if (!currentMusic.isValid()) return;

        float target = enable ? 1f : 0f;
        float duration = enable ? 0.6f : 1.8f;

        Debug.Log($"AudioManager: Modes → {target} ({duration}s)");
        SetMusicParameterSmooth("Modes", target, duration);
    }

    private bool IsPlayingMusic(EventReference musicEvent)
    {
        if (!currentMusic.isValid() || musicEvent.IsNull) return false;

        try
        {
            EventDescription currentDesc;
            currentMusic.getDescription(out currentDesc);

            string currentPath;
            currentDesc.getPath(out currentPath);

            EventInstance tempInstance = RuntimeManager.CreateInstance(musicEvent);
            EventDescription targetDesc;
            tempInstance.getDescription(out targetDesc);

            string targetPath;
            targetDesc.getPath(out targetPath);

            tempInstance.release();

            return !string.IsNullOrEmpty(currentPath) &&
                   !string.IsNullOrEmpty(targetPath) &&
                   currentPath == targetPath;
        }
        catch
        {
            PLAYBACK_STATE playbackState;
            currentMusic.getPlaybackState(out playbackState);
            return playbackState == PLAYBACK_STATE.PLAYING;
        }
    }

    #endregion

  
}