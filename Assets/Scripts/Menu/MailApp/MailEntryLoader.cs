using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class MailEntryLoader : MonoBehaviour
{
    [SerializeField] public MailLists m_mailLists = new MailLists();

    [Header("Mail Loading")]
    public GameObject m_mailEntryPrefab;
    public GameObject m_MailContent;
    public GameObject m_mailViewPortContent;
    private List<GameObject> m_spawnedMails = new List<GameObject>();

    [Header("Notifications")]
    public GameObject m_notificationGameObject;
    public TextMeshProUGUI m_textUGUI;
    private float m_clickCounter = 0;
    private float m_timeToDoubleClick = 1.5f;
    public int m_loadedMails { get; private set; }
    [SerializeField] private int m_newMails;

    [System.Serializable]
    public class MailJsonData
    {
        public string m_senderPFPstring;
        public string m_senderArroba;
        public string m_emailContent;
        public string m_senderName;
        public string m_senderDate;
        public int m_level;
        public string m_extraImage;
    }

    [System.Serializable]
    public class MailLists { public MailJsonData[] mail; }

    void OnEnable()
    {
        LocalizationManager.OnLanguageChanged += ReloadAllMails;
    }

    void OnDisable()
    {
        LocalizationManager.OnLanguageChanged -= ReloadAllMails;
    }

    void Start()
    {
        ReloadAllMails();
    }

    public void ReloadAllMails()
    {
        foreach (GameObject go in m_spawnedMails) Destroy(go);
        m_spawnedMails.Clear();
        m_loadedMails = 0;
        m_newMails = 0;

        string fileName = "Mails_" + GabeNewell.Instance.m_Language;
        TextAsset jsonFile = Resources.Load<TextAsset>(fileName);

        if (jsonFile != null)
        {
            m_mailLists = JsonUtility.FromJson<MailLists>(jsonFile.text);
            
            for (int i = 0; i < m_mailLists.mail.Length; i++)
            {
                if (m_mailLists.mail[i].m_level == 6969 && GabeNewell.Instance.m_MaxTimeMinesweeper) {
                    CreateMailEntry(i);
                }
                if (m_mailLists.mail[i].m_level <= GabeNewell.Instance.m_Level())
                {
                    CreateMailEntry(i);
                }
            }
            if (!GabeNewell.Instance.m_MailsAreRead) Notifiy(); 
        }
    }

    private void CreateMailEntry(int index)
    {
        GameObject m_contentBox = Instantiate(m_mailEntryPrefab);
        m_spawnedMails.Add(m_contentBox);
        m_contentBox.transform.SetParent(m_mailViewPortContent.transform, false);

        RectTransform rt = m_contentBox.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        Mail l_mail = m_contentBox.GetComponent<Mail>();
        l_mail.SetValues(
            m_mailLists.mail[index].m_senderPFPstring,
            m_mailLists.mail[index].m_senderArroba,
            m_mailLists.mail[index].m_emailContent,
            m_mailLists.mail[index].m_senderName,
            m_mailLists.mail[index].m_senderDate,
            m_MailContent,
            m_mailLists.mail[index].m_extraImage,
            this,
            m_mailLists.mail[index].m_level
        );

        rt.anchoredPosition = new Vector2(-105, -50 * m_loadedMails);
        if (m_mailLists.mail[index].m_level < GabeNewell.Instance.m_Level()) {l_mail.Opened(); l_mail.OnClick(); }
        else m_newMails++;

        m_loadedMails++;
    }

    public void Notifiy()
    {
        m_textUGUI.text = m_newMails.ToString();
        m_notificationGameObject.SetActive(true);
        GabeNewell.Instance.m_MailsAreRead = false;
    }

    public void OpenMail()
    {
        if (m_clickCounter < m_timeToDoubleClick && !GabeNewell.Instance.m_MailsAreRead)
        {
            m_textUGUI.text = m_newMails.ToString();
        }
        m_clickCounter = 0;
    }

    public void OpenedNewEntry()
    {
        m_newMails--;
        m_textUGUI.text = m_newMails.ToString();
        if (m_newMails <= 0)
        {
            GabeNewell.Instance.m_MailsAreRead=true;
            m_notificationGameObject.SetActive(false);
        }
    }
}