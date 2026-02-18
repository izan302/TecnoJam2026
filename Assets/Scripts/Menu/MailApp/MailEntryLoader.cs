using TMPro;
using UnityEngine;

public class MailEntryLoader : MonoBehaviour
{
    public TextAsset m_textJSON;
    [SerializeField]public MailLists m_mailLists = new MailLists();

    [Header("Mail Loading")]
    public GameObject m_mailEntryPrefab;
    public GameObject m_MailContent;
    public GameObject m_mailViewPortContent;
    private GameObject m_contentBox;
    [Header("Notifications")]
    public GameObject m_notificationGameObject;
    public TextMeshProUGUI m_textUGUI;
    private float m_clickCounter = 0;
    private float m_timeToDoubleClick = 1.5f;  
    public int m_loadedMails { get; private set; }
    [SerializeField]private int m_newMails;


    [System.Serializable]
    public class MailJsonData
    {
        public string m_senderPFPstring;
        public string m_senderArroba;
        public string m_emailContent;
        public string m_senderName;
        public string m_senderDate;
        public int m_level;
    }
    public class MailLists
    {
        public MailJsonData[] mail;
    }

    void Start()
    {
        m_newMails = 0;
        m_mailLists = JsonUtility.FromJson<MailLists>(m_textJSON.text);
        for (int i = 0; i<m_mailLists.mail.Length; i++)
        {
            if (m_mailLists.mail[i].m_level != 0)
            {
                continue;
            }
            m_contentBox = Instantiate(m_mailEntryPrefab);
            m_contentBox.transform.SetParent(m_mailViewPortContent.transform);
            Mail l_mail = m_contentBox.GetComponent<Mail>();
            l_mail.SetValues(m_mailLists.mail[i].m_senderPFPstring, m_mailLists.mail[i].m_senderArroba, m_mailLists.mail[i].m_emailContent, m_mailLists.mail[i].m_senderName, m_mailLists.mail[i].m_senderDate, m_MailContent);
            RectTransform l_rectTransform = m_contentBox.GetComponent<RectTransform>();
            l_rectTransform.anchoredPosition = new Vector3(-92, -50 * m_loadedMails, 0);
            m_loadedMails++;
            m_newMails++;
        }
        Notifiy();
    }

    public void LoadLevelMail(int level)
    {
        for (int i = 0; i<m_mailLists.mail.Length; i++)
        {
            if (m_mailLists.mail[i].m_level == level)
            {
                m_contentBox = Instantiate(m_mailEntryPrefab);
                m_contentBox.transform.SetParent(m_mailViewPortContent.transform);
                Mail l_mail = m_contentBox.GetComponent<Mail>();
                l_mail.SetValues(m_mailLists.mail[i].m_senderPFPstring, m_mailLists.mail[i].m_senderArroba, m_mailLists.mail[i].m_emailContent, m_mailLists.mail[i].m_senderName, m_mailLists.mail[i].m_senderDate, m_MailContent);
                RectTransform l_rectTransform = m_contentBox.GetComponent<RectTransform>();
                l_rectTransform.anchoredPosition = new Vector3(-92, -50 * m_loadedMails, 0);
                m_loadedMails++;
                m_newMails++;
            }
        }
        Notifiy();
    }
    public void Notifiy()
    {
        m_textUGUI.text = m_newMails.ToString();
        m_notificationGameObject.SetActive(true);
    }
    public void OpenMail()
    {
        if (m_clickCounter < m_timeToDoubleClick)
        {
            m_newMails = 0;
            m_textUGUI.text = m_newMails.ToString();
            m_notificationGameObject.SetActive(false);
        }
        m_clickCounter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        m_clickCounter += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadLevelMail(1);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadLevelMail(2);
        }
    }
}
