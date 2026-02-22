using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LocalizedText : MonoBehaviour
{
    [SerializeField] private string key;
    private TextMeshProUGUI m_Text;

    void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        UpdateText();
        LocalizationManager.OnLanguageChanged += UpdateText;
    }

    void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= UpdateText;
    }

    public void UpdateText()
    {
        if (LocalizationManager.instance != null)
        {
            m_Text.text = LocalizationManager.instance.GetText(key);
        }
    }
}