using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    private Dictionary<string, string> localizedText;
    
    public static event Action OnLanguageChanged;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLanguage();
        }
        else { Destroy(gameObject); }
    }

    public void LoadLanguage()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Localization");
        if (jsonFile == null) return;

        LocalizationData data = JsonUtility.FromJson<LocalizationData>(jsonFile.text);
        localizedText = new Dictionary<string, string>();

        string targetLang = GabeNewell.Instance.m_Language;
        LanguageGroup selectedGroup = data.languages.FirstOrDefault(x => x.languageID == targetLang);

        if (selectedGroup != null)
        {
            foreach (var item in selectedGroup.items)
            {
                localizedText[item.key] = item.value;
            }
        }

        OnLanguageChanged?.Invoke();
    }

    public string GetText(string key)
    {
        if (localizedText != null && localizedText.ContainsKey(key)) return localizedText[key];
        return key;
    }
}

[System.Serializable]
public class LocalizationItem { public string key; public string value; }

[System.Serializable]
public class LanguageGroup { public string languageID; public List<LocalizationItem> items; }

[System.Serializable]
public class LocalizationData { public List<LanguageGroup> languages; }