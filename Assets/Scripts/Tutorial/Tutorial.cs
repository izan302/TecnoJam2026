using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private TextMeshProUGUI m_TextComponent;
    [SerializeField] private float m_CharactersPerSecond = 30f;
    [SerializeField] private int m_MouthSpeedDivisor = 3; 
    [SerializeField] private float m_RotationAmount = 5f;
    [SerializeField] private float m_RotationSpeed = 10f;

    [Header("Localización")]
    [SerializeField] private string[] m_TutorialTextKeys; 

    [Header("Referencias")]
    [SerializeField] GameObject m_SupplementaryGridIndicator;
    [SerializeField] GameObject m_GridIndicator;
    [SerializeField] GameObject m_ScreenIndicator;

    [Header("Portrait")]
    [SerializeField] GameObject m_Face;
    [SerializeField] GameObject m_Mouth;
    [SerializeField] GameObject m_AngryFace;

    private Vector3 m_MouthOriginalLocalPos;
    private Quaternion m_FaceOriginalRotation;
    private int m_CurrentIndex = 0;
    private Coroutine m_TypeRoutine;
    private bool m_IsTyping = false;
    private bool m_IsAngryMode = false;
    private int m_TotalVisibleCharacters;
    private string m_ActiveKey;
    private RawImage m_FaceImage;

    void OnEnable() => LocalizationManager.OnLanguageChanged += RefreshLocalizedText;
    void OnDisable() => LocalizationManager.OnLanguageChanged -= RefreshLocalizedText;

    void Awake()
    {
        m_MouthOriginalLocalPos = m_Mouth.transform.localPosition;
        m_FaceOriginalRotation = m_Face.transform.localRotation;
        m_FaceImage = m_Face.GetComponent<RawImage>();
        
        m_AngryFace.SetActive(false);
        if (m_FaceImage != null) m_FaceImage.enabled = true;
    }

    void Start()
    {
        ResetIndicators();
        
        if (!GabeNewell.Instance.m_TutorialPlayed)
        {
            GabeNewell.Instance.m_IsTutorialPlaying = true;
            m_CurrentIndex = 0;
            m_IsAngryMode = false;
            ShowText(m_TutorialTextKeys[m_CurrentIndex]);
        }
        else
        {
            m_TextComponent.text = "";
            if (m_FaceImage != null) m_FaceImage.enabled = false;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        bool l_HasInput = Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space);
        
        if (l_HasInput)
        {
            if (m_IsTyping)
            {
                CompleteTextInstantly();
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
    }

    public void ShowText(string _key)
    {
        if (m_TypeRoutine != null) StopCoroutine(m_TypeRoutine);
        
        m_ActiveKey = _key;
        m_TextComponent.text = LocalizationManager.instance.GetText(_key);
        m_TextComponent.maxVisibleCharacters = 0;
        m_TextComponent.ForceMeshUpdate(); 
        m_TotalVisibleCharacters = m_TextComponent.textInfo.characterCount;

        m_IsTyping = true;
        m_TypeRoutine = StartCoroutine(TypeTextRoutine());
    }

    public void AngryText(string _TextKey)
    {
        m_IsAngryMode = true;
        gameObject.SetActive(true);
        
        m_AngryFace.SetActive(true);
        if (m_FaceImage != null) m_FaceImage.enabled = false;
        
        ShowText(_TextKey);
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
            FinalizarTutorial();
        }
    }

    private IEnumerator TypeTextRoutine()
    {
        int i_Counter = 0;
        float l_StartTime = Time.time;

        while (i_Counter <= m_TotalVisibleCharacters)
        {
            float l_RotZ = Mathf.Sin((Time.time - l_StartTime) * m_RotationSpeed) * m_RotationAmount;
            m_Face.transform.localRotation = m_FaceOriginalRotation * Quaternion.Euler(0, 0, l_RotZ);

            if (i_Counter % m_MouthSpeedDivisor == 0)
            {
                bool l_Open = (i_Counter / m_MouthSpeedDivisor % 2 != 0);
                m_Mouth.transform.localPosition = l_Open ? 
                    m_MouthOriginalLocalPos - new Vector3(0, 0.02f, 0) : 
                    m_MouthOriginalLocalPos;
            }

            m_TextComponent.maxVisibleCharacters = i_Counter;
            i_Counter++;
            yield return new WaitForSeconds(1f / m_CharactersPerSecond);
        }

        OnTypingFinished();
    }

    private void CompleteTextInstantly()
    {
        if (m_TypeRoutine != null) StopCoroutine(m_TypeRoutine);
        m_TextComponent.maxVisibleCharacters = m_TotalVisibleCharacters;
        OnTypingFinished();
    }

    private void OnTypingFinished()
    {
        m_IsTyping = false;
        m_Mouth.transform.localPosition = m_MouthOriginalLocalPos;
        m_Face.transform.localRotation = m_FaceOriginalRotation;

        if (!m_IsAngryMode && GabeNewell.Instance.m_IsTutorialPlaying)
        {
            UpdateTutorialVisuals(m_CurrentIndex);
        }
    }

    private void UpdateTutorialVisuals(int _index)
    {
        m_SupplementaryGridIndicator.SetActive(_index == 1);
        m_GridIndicator.SetActive(_index == 2);
        m_ScreenIndicator.SetActive(_index == 3);
    }

    private void ResetAfterAngry()
    {
        m_IsAngryMode = false;
        m_AngryFace.SetActive(false);
        m_TextComponent.text = "";
        
        if (!GabeNewell.Instance.m_IsTutorialPlaying) 
        {
            if (m_FaceImage != null) m_FaceImage.enabled = false;
            gameObject.SetActive(false);
        }
        else
        {
            if (m_FaceImage != null) m_FaceImage.enabled = true;
        }
    }

    private void ResetIndicators()
    {
        m_SupplementaryGridIndicator.SetActive(false);
        m_GridIndicator.SetActive(false);
        m_ScreenIndicator.SetActive(false);
    }

    private void FinalizarTutorial()
    {
        m_TextComponent.text = "";
        GabeNewell.Instance.m_IsTutorialPlaying = false;
        GabeNewell.Instance.m_TutorialPlayed = true;
        if (m_FaceImage != null) m_FaceImage.enabled = false;
        gameObject.SetActive(false);
    }

    private void RefreshLocalizedText()
    {
        if (!m_IsTyping && !string.IsNullOrEmpty(m_ActiveKey))
            m_TextComponent.text = LocalizationManager.instance.GetText(m_ActiveKey);
    }
}