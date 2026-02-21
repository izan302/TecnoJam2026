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

    [Header("Textos")]
    [SerializeField, TextArea(3,5)] private string[] m_TutorialTexts; 

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
    private int m_TotalVisibleCharacters;

    void Start()
    {
        m_MouthOriginalLocalPos = m_Mouth.transform.localPosition;
        m_FaceOriginalRotation = m_Face.transform.localRotation;

        m_SupplementaryGridIndicator.SetActive(false);
        m_GridIndicator.SetActive(false);
        m_ScreenIndicator.SetActive(false);
        m_AngryFace.SetActive(false);
        
        if (GabeNewell.Instance.m_Level == 1)
        {
            GabeNewell.Instance.m_IsTutorialPlaying = true;
            StartCoroutine(ShowNextText());
        }
            
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) && GabeNewell.Instance.m_Level == 1)
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        if (m_IsTyping)
        {
            StopCoroutine(m_TypeRoutine);
            m_TextComponent.maxVisibleCharacters = m_TotalVisibleCharacters;
            m_Mouth.transform.localPosition = m_MouthOriginalLocalPos;
            m_Face.transform.localRotation = m_FaceOriginalRotation;
            m_IsTyping = false;
            OnTextFinished(m_CurrentIndex);
        }
        else
        {
            m_CurrentIndex++;
            if (m_CurrentIndex < m_TutorialTexts.Length)
            {
                StartCoroutine(ShowNextText());
            }
            else
            {
                FinalizarTutorial();
            }
        }
    }

    private IEnumerator ShowNextText()
    {
        m_SupplementaryGridIndicator.SetActive(false);
        m_GridIndicator.SetActive(false);
        m_ScreenIndicator.SetActive(false);

        m_TextComponent.text = m_TutorialTexts[m_CurrentIndex];
        m_TextComponent.maxVisibleCharacters = 0;
        
        m_TextComponent.ForceMeshUpdate(); 
        m_TotalVisibleCharacters = m_TextComponent.textInfo.characterCount;

        m_IsTyping = true;
        m_TypeRoutine = StartCoroutine(TypeText());
        yield return null;
    }

    private IEnumerator ShowText(String _Text)
    {
        m_SupplementaryGridIndicator.SetActive(false);
        m_GridIndicator.SetActive(false);
        m_ScreenIndicator.SetActive(false);

        m_TextComponent.text = _Text;
        m_TextComponent.maxVisibleCharacters = 0;
        
        m_TextComponent.ForceMeshUpdate(); 
        m_TotalVisibleCharacters = m_TextComponent.textInfo.characterCount;

        m_IsTyping = true;
        m_TypeRoutine = StartCoroutine(TypeSpecificText(-1));
        yield return null;
    }

    private IEnumerator TypeSpecificText(int _Index)
    {
        int i_Counter = 0;
        bool l_IsMouthOpen = false;
        float l_StartTime = Time.time;

        while (i_Counter <= m_TotalVisibleCharacters)
        {
            float l_RotationZ = Mathf.Sin((Time.time - l_StartTime) * m_RotationSpeed) * m_RotationAmount;
            m_Face.transform.localRotation = m_FaceOriginalRotation * Quaternion.Euler(0, 0, l_RotationZ);

            if (i_Counter % m_MouthSpeedDivisor == 0)
            {
                m_Mouth.transform.localPosition = l_IsMouthOpen ? 
                    m_MouthOriginalLocalPos - new Vector3(0, 0.02f, 0) : 
                    m_MouthOriginalLocalPos;

                l_IsMouthOpen = !l_IsMouthOpen;
            }

            m_TextComponent.maxVisibleCharacters = i_Counter;
            i_Counter++;
            yield return new WaitForSeconds(1f / m_CharactersPerSecond);
        }

        m_Mouth.transform.localPosition = m_MouthOriginalLocalPos;
        m_Face.transform.localRotation = m_FaceOriginalRotation;
        m_IsTyping = false;
        OnTextFinished(_Index);
    }

    private IEnumerator TypeText()
    {
        int i_Counter = 0;
        bool l_IsMouthOpen = false;
        float l_StartTime = Time.time;

        while (i_Counter <= m_TotalVisibleCharacters)
        {
            float l_RotationZ = Mathf.Sin((Time.time - l_StartTime) * m_RotationSpeed) * m_RotationAmount;
            m_Face.transform.localRotation = m_FaceOriginalRotation * Quaternion.Euler(0, 0, l_RotationZ);

            if (i_Counter % m_MouthSpeedDivisor == 0)
            {
                m_Mouth.transform.localPosition = l_IsMouthOpen ? 
                    m_MouthOriginalLocalPos - new Vector3(0, 0.02f, 0) : 
                    m_MouthOriginalLocalPos;

                l_IsMouthOpen = !l_IsMouthOpen;
            }

            m_TextComponent.maxVisibleCharacters = i_Counter;
            i_Counter++;
            yield return new WaitForSeconds(1f / m_CharactersPerSecond);
        }

        m_Mouth.transform.localPosition = m_MouthOriginalLocalPos;
        m_Face.transform.localRotation = m_FaceOriginalRotation;
        m_IsTyping = false;
        OnTextFinished(m_CurrentIndex);
    }

    private void OnTextFinished(int i_Index)
    {
        switch (i_Index)
        {
            case 1: m_SupplementaryGridIndicator.SetActive(true); break;
            case 2: m_GridIndicator.SetActive(true); break;
            case 3: m_ScreenIndicator.SetActive(true); break;
            case -1: break;
        }
    }

    private void FinalizarTutorial()
    {
        m_TextComponent.text = "";
        GabeNewell.Instance.m_IsTutorialPlaying = false;
        GabeNewell.Instance.m_TutorialPlayed = true;
        gameObject.SetActive(false);
    }

    public void AngryText(String _Text)
    {
        StartCoroutine(ShowText(_Text));
        m_AngryFace.SetActive(true);
        m_Face.GetComponent<RawImage>().enabled = false;
    }
}