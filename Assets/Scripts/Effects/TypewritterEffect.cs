using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private TextMeshProUGUI m_TextComponent;
    [SerializeField] private float m_CharactersPerSecond = 30f;
    [SerializeField] private bool m_PauseOnPunctuation = true;
    [SerializeField] private float m_PunctuationPauseTime = 0.2f;

    public event Action OnTextFinished;

    private Coroutine m_TypeRoutine;
    private int m_TotalCharacters;
    private bool m_IsTyping = false;

    public void PlayText(string _textKey, TextMeshProUGUI _textComponent)
    {
        if (m_TypeRoutine != null) StopCoroutine(m_TypeRoutine);

        string l_translatedText = LocalizationManager.instance.GetText(_textKey);
        m_TextComponent = _textComponent;
        m_TextComponent.text = l_translatedText;
        m_TextComponent.maxVisibleCharacters = 0;
        
        m_TextComponent.ForceMeshUpdate();
        m_TotalCharacters = m_TextComponent.textInfo.characterCount;

        m_IsTyping = true;
        m_TypeRoutine = StartCoroutine(TypeTextRoutine());
    }

    private IEnumerator TypeTextRoutine()
    {
        int l_counter = 0;
        float l_waitBase = 1f / m_CharactersPerSecond;

        while (l_counter <= m_TotalCharacters)
        {
            m_TextComponent.maxVisibleCharacters = l_counter;

            if (m_PauseOnPunctuation && l_counter > 0 && l_counter <= m_TotalCharacters)
            {
                char l_lastChar = m_TextComponent.textInfo.characterInfo[Mathf.Max(0, l_counter - 1)].character;
                if (IsPunctuation(l_lastChar))
                {
                    yield return new WaitForSeconds(m_PunctuationPauseTime);
                }
            }

            l_counter++;
            yield return new WaitForSeconds(l_waitBase);
        }

        FinishTyping();
    }

    private bool IsPunctuation(char _c)
    {
        return _c == '.' || _c == ',' || _c == '!' || _c == '?' || _c == ':' || _c == ';';
    }

    public void Skip()
    {
        if (!m_IsTyping) return;

        if (m_TypeRoutine != null) StopCoroutine(m_TypeRoutine);
        m_TextComponent.maxVisibleCharacters = m_TotalCharacters;
        FinishTyping();
    }

    private void FinishTyping()
    {
        m_IsTyping = false;
        m_TypeRoutine = null;
        OnTextFinished?.Invoke();
    }
}