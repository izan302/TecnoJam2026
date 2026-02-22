using UnityEngine;
using TMPro;
using System.Collections.Generic;

[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageDropdown : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;

    void Start()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Localization");
        if (jsonFile == null) return;

        LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonFile.text);
        
        m_Dropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < data.languages.Count; i++)
        {
            string langID = data.languages[i].languageID;
            options.Add(langID);

            if (langID == GabeNewell.Instance.m_Language)
            {
                currentIndex = i;
            }
        }

        m_Dropdown.AddOptions(options);
        m_Dropdown.value = currentIndex;
        m_Dropdown.RefreshShownValue();

        m_Dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    private void OnDropdownChanged(int index)
    {
        string selectedLang = m_Dropdown.options[index].text;
        
        GabeNewell.Instance.m_Language = selectedLang;
        LocalizationManager.instance.LoadLanguage();
    }
}