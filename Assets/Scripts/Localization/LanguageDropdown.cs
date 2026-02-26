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

    private static Dictionary<string, TMP_Dropdown.OptionData> options;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();

        if (options == null)
            options = new Dictionary<string, TMP_Dropdown.OptionData>();
    }

    private void Start()
    {
        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Localization");
        if (jsonFile == null)
        {
            Debug.LogError("Localization.json not found in Resources");
            return;
        }

        LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonFile.text);

        m_Dropdown.ClearOptions();
        options.Clear();

        foreach (var lang in data.languages)
        {
            string langID = lang.languageID;

            Sprite flag = null;
            if (flagIcons.ContainsKey(langID))
                flag = flagIcons[langID];

            var option = new TMP_Dropdown.OptionData(langID, flag, Color.white);
            options.Add(langID, option);
        }

        List<TMP_Dropdown.OptionData> orderedOptions = new List<TMP_Dropdown.OptionData>();

        foreach (var lang in data.languages)
        {
            orderedOptions.Add(options[lang.languageID]);
        }

        m_Dropdown.AddOptions(orderedOptions);

        SetDropdownValue();

        m_Dropdown.onValueChanged.RemoveAllListeners();
        m_Dropdown.onValueChanged.AddListener(OnDropdownChanged);
    }

    void SetDropdownValue()
    {
        string currentLang = GabeNewell.Instance.m_Language;

        int index = 0;

        for (int i = 0; i < m_Dropdown.options.Count; i++)
        {
            if (m_Dropdown.options[i].text == currentLang)
            {
                index = i;
                break;
            }
        }

        m_Dropdown.SetValueWithoutNotify(index);
        m_Dropdown.RefreshShownValue();
    }

    private void OnDropdownChanged(int index)
    {
        string selectedLang = m_Dropdown.options[index].text;

        GabeNewell.Instance.m_Language = selectedLang;
        LocalizationManager.instance.LoadLanguage();
    }
}