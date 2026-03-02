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
    public EventReference GameSent;

    [Header("SFX")]
    public EventReference settings;
    public EventReference start;
    public EventReference click;
    public EventReference closing;
    public EventReference error;
    public EventReference carta;
    public EventReference sendButton;






    [Header("Cinematicas")]

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private EventInstance currentMusic;
    private EventInstance nextMusic;
    private bool isTransitioning = false;

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
        currentMusic = RuntimeManager.CreateInstance(menuMusic);
        currentMusic.start();
        currentMusic.setVolume(musicVolume);
    }

    private void SetupLowLatency()
    {
        try
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();
            config.dspBufferSize = 256;
            AudioSettings.Reset(config);

            Debug.Log("Configuración de audio de Unity ajustada para baja latencia");
        }
        catch
        {
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


    public void ChangeMusicWithFade(EventReference newMusic, float fadeTime = 2f)
    {
        if (newMusic.IsNull || isTransitioning) return;
        StartCoroutine(SmoothCrossfadeMusic(newMusic, fadeTime));
    }

    private IEnumerator SmoothCrossfadeMusic(EventReference newMusic, float fadeTime)
    {
        if (newMusic.IsNull) yield break;

        if (IsPlayingMusic(newMusic))
        {
            if (currentMusic.isValid())
            {
                currentMusic.setVolume(musicVolume);
            }
            yield break;
        }

        isTransitioning = true;

        float startVolume = musicVolume;

        nextMusic = RuntimeManager.CreateInstance(newMusic);
        nextMusic.start();
        nextMusic.setVolume(0f);

        float timer = 0f;

        while (timer < fadeTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeTime);

            float smoothT = Mathf.SmoothStep(0f, 1f, t);
            float exponentialT = 1f - Mathf.Pow(1f - t, 3f);
            float finalT = (smoothT + exponentialT) * 0.5f;

            if (currentMusic.isValid())
            {
                float oldVolume = Mathf.Lerp(startVolume, 0f, finalT);
                currentMusic.setVolume(oldVolume);
            }

            float newVolume = Mathf.Lerp(0f, startVolume, finalT);
            nextMusic.setVolume(newVolume);

            yield return null;
        }

        if (currentMusic.isValid())
        {
            currentMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

        currentMusic = nextMusic;
        nextMusic.clearHandle();
        isTransitioning = false;
    }

    public void SetMusicParameterSmooth(string parameterName, float targetValue, float duration = 1.5f)
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
            float elasticT = Mathf.Sin(t * Mathf.PI * 0.5f);
            float value = Mathf.Lerp(startValue, targetValue, (smoothT + elasticT) * 0.5f);

            currentMusic.setParameterByName(parameterName, value);

            currentModesValue = value;
            yield return null;
        }

        currentModesValue = targetValue;
        currentMusic.setParameterByName(parameterName, targetValue);
    }

    #endregion

    #region SFX

    public void PlayOneShot(EventReference sound)
    {
        if (!sound.IsNull)
            RuntimeManager.PlayOneShot(sound);
    }
    
    public void PlaySettings ()
    {
            RuntimeManager.PlayOneShot(settings);

    }

    public void Closing ()
    {
        RuntimeManager.PlayOneShot(closing);

    }

    public void PlayStartSound ()
    {
        RuntimeManager.PlayOneShot(start);

    }

    public void PlayClick ()
    {
        if (SceneManager.GetActiveScene().name == "GameplayScene" | SceneManager.GetActiveScene().name == "DesktopScene")
        {
            RuntimeManager.PlayOneShot(click);
        }
    }

    public void Error()
    {
        RuntimeManager.PlayOneShot(error);

    }

    public void DesplegarCarta ()
    {
        RuntimeManager.PlayOneShot(carta);

    }

    public void SendButton ()
    {
        RuntimeManager.PlayOneShot(sendButton);

    }

    #endregion

    #region Cambios de Escena

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return;
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
                Debug.Log("AudioManager: Música de Desktop");
                ApplyShopLowcut(false);
                break;
            case "GameplayScene":
                newMusic = gameplayMusic;
                Debug.Log("AudioManager: Música de Gameplay");
                ApplyShopLowcut(false);
                break;
                case "GameSentScene":
                newMusic = GameSent;
                Debug.Log("AudioManager: Música de Gameplay");
                ApplyShopLowcut(false);
                break;
        }

        if (!newMusic.IsNull && !IsPlayingMusic(newMusic) && !isTransitioning)
        {
            Debug.Log($"AudioManager: Cambiando a nueva música con transición suave");
            StartCoroutine(SmoothCrossfadeMusic(newMusic, 2f));
        }
    }

    private void ApplyShopLowcut(bool enable)
    {
        if (!currentMusic.isValid()) return;

        float target = enable ? 1f : 0f;
        float duration = enable ? 1.2f : 2.5f;

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