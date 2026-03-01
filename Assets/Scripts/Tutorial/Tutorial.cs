using TMPro;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity; 

public class Tutorial : MonoBehaviour
{
    [Header("Configuración Animación")]
    [SerializeField] private float m_RotationAmount = 5f;
    [SerializeField] private float m_RotationSpeed = 10f;
    [SerializeField] private float m_MouthMoveAmount = 10f;
    [SerializeField] private float m_MouthSpeed = 15f;

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI m_TextComponent;
    [SerializeField] private TypewriterEffect m_TypewriterEffect;

    [Header("Retratos y Bocas")]
    [SerializeField] private GameObject m_NormalFaceGroup;
    [SerializeField] private GameObject m_NormalMouth;
    [SerializeField] private GameObject m_AngryFaceGroup;
    [SerializeField] private GameObject m_AngryMouth;

    [Header("Indicadores Visuales")]
    [SerializeField] private GameObject m_SupplementaryGridIndicator;
    [SerializeField] private GameObject m_GridIndicator;
    [SerializeField] private GameObject m_ScreenIndicator;

    [Header("Localización")]
    [SerializeField] private string[] m_TutorialTextKeys;

    [Header("FMOD Audio")] 
    [SerializeField] private EventReference m_TypingSound; 

    private Vector3 m_NormalMouthOriginalPos;
    private Vector3 m_AngryMouthOriginalPos;
    private Quaternion m_NormalOriginalRot;
    private Quaternion m_AngryOriginalRot;

    private int m_CurrentIndex = 0;
    private bool m_IsAngryMode = false;

    private float m_NextSoundTime = 0f;
    [SerializeField] private float m_SoundInterval = 0.05f; 

    void Awake()
    {
        m_NormalMouthOriginalPos = m_NormalMouth.transform.localPosition;
        m_AngryMouthOriginalPos = m_AngryMouth.transform.localPosition;
        m_NormalOriginalRot = m_NormalFaceGroup.transform.localRotation;
        m_AngryOriginalRot = m_AngryFaceGroup.transform.localRotation;

        ResetIndicators();
    }

    void Start()
    {
        m_TypewriterEffect.OnTextFinished += OnTypingFinished;

        if (!GabeNewell.Instance.m_TutorialPlayed && m_TutorialTextKeys.Length > 0)
        {
            GabeNewell.Instance.m_IsTutorialPlaying = true;
            m_CurrentIndex = 0;
            ShowText(m_TutorialTextKeys[m_CurrentIndex]);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool l_HasInput = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);

        if (l_HasInput)
        {
            if (m_TypewriterEffect.IsTyping)
            {
                m_TypewriterEffect.Skip();
            }
            else if (m_IsAngryMode)
            {
                ResetAfterAngry();
            }
            else if (GabeNewell.Instance.m_IsTutorialPlaying)
            {
                AdvanceTutorial();
            }
        }

        if (m_TypewriterEffect.IsTyping)
        {
            AnimateTalking();

            PlayTypingSoundLoop();
        }
        else
        {
            ResetAnimation();
        }
    }

    private void PlayTypingSoundLoop()
    {
        if (Time.time >= m_NextSoundTime)
        {
            if (!m_TypingSound.IsNull)
            {
                RuntimeManager.PlayOneShot(m_TypingSound, transform.position);
            }
            m_NextSoundTime = Time.time + m_SoundInterval;
        }
    }

    public void ShowText(string _key)
    {
        SetAngryMode(false);
        m_TypewriterEffect.PlayText(_key, m_TextComponent);
        m_NextSoundTime = 0f; 
    }

    public void AngryText(string _key)
    {
        gameObject.SetActive(true);
        SetAngryMode(true);
        m_TypewriterEffect.PlayText(_key, m_TextComponent);
        m_NextSoundTime = 0f; 
    }

    private void AdvanceTutorial()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex < m_TutorialTextKeys.Length)
        {
            ShowText(m_TutorialTextKeys[m_CurrentIndex]);
        }
        else
        {
            FinishTutorial();
        }
    }

    private void OnTypingFinished()
    {
        ResetAnimation();
        if (!m_IsAngryMode)
        {
            UpdateTutorialVisuals(m_CurrentIndex);
        }
    }

    private void AnimateTalking()
    {
        float l_RotZ = Mathf.Sin(Time.time * m_RotationSpeed) * m_RotationAmount;
        float l_MouthOffset = Mathf.PingPong(Time.time * m_MouthSpeed, m_MouthMoveAmount);

        if (!m_IsAngryMode)
        {
            m_NormalFaceGroup.transform.localRotation = m_NormalOriginalRot * Quaternion.Euler(0, 0, l_RotZ);
            m_NormalMouth.transform.localPosition = m_NormalMouthOriginalPos + new Vector3(0, -l_MouthOffset, 0);
        }
        else
        {
            m_AngryFaceGroup.transform.localRotation = m_AngryOriginalRot * Quaternion.Euler(0, 0, l_RotZ);
            m_AngryMouth.transform.localPosition = m_AngryMouthOriginalPos + new Vector3(0, -l_MouthOffset, 0);
        }
    }

    private void ResetAnimation()
    {
        m_NormalFaceGroup.transform.localRotation = m_NormalOriginalRot;
        m_NormalMouth.transform.localPosition = m_NormalMouthOriginalPos;
        m_AngryFaceGroup.transform.localRotation = m_AngryOriginalRot;
        m_AngryMouth.transform.localPosition = m_AngryMouthOriginalPos;
    }

    public void SetAngryMode(bool _isAngry)
    {
        m_IsAngryMode = _isAngry;
        m_NormalFaceGroup.SetActive(!_isAngry);
        m_AngryFaceGroup.SetActive(_isAngry);
    }

    private void ResetAfterAngry()
    {
        SetAngryMode(false);
        m_TextComponent.text = "";

        if (GabeNewell.Instance.m_TutorialPlayed)
        {
            gameObject.SetActive(false);
        }
    }

    private void UpdateTutorialVisuals(int _index)
    {
        if (m_SupplementaryGridIndicator) m_SupplementaryGridIndicator.SetActive(_index == 1);
        if (m_GridIndicator) m_GridIndicator.SetActive(_index == 2);
        if (m_ScreenIndicator) m_ScreenIndicator.SetActive(_index == 3);
    }

    private void ResetIndicators()
    {
        if (m_SupplementaryGridIndicator) m_SupplementaryGridIndicator.SetActive(false);
        if (m_GridIndicator) m_GridIndicator.SetActive(false);
        if (m_ScreenIndicator) m_ScreenIndicator.SetActive(false);
    }

    private void FinishTutorial()
    {
        GabeNewell.Instance.m_IsTutorialPlaying = false;
        GabeNewell.Instance.m_TutorialPlayed = true;
        m_TextComponent.text = "";
        ResetIndicators();
        gameObject.SetActive(false);
    }
}