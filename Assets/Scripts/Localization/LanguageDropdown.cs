using UnityEngine;
using TMPro;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using System;
[RequireComponent(typeof(TMP_Dropdown))]
public class LanguageDropdown : MonoBehaviour
{
    private TMP_Dropdown m_Dropdown;
    public SerializedDictionary<string, Sprite> flagIcons;

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
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        int currentIndex = 0;

        for (int i = 0; i < data.languages.Count; i++)
        {
            string langID = data.languages[i].languageID;

            Sprite flag = null;
            flagIcons.TryGetValue(langID, out flag);

            var option =  new TMP_Dropdown.OptionData(langID, flag, Color.white);
            options.Add(option);

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