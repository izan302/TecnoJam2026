using TMPro;
using UnityEngine;
using static Mail;

public class MailEntryLoader : MonoBehaviour
{
    public TextAsset m_textJSON;
    [SerializeField] public MailLists m_mailLists = new MailLists();

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

    public class MailLists { public MailJsonData[] mail; }

    void Start()
    {
        m_newMails = 0;
        m_mailLists = JsonUtility.FromJson<MailLists>(m_textJSON.text);
        
        for (int i = 0; i < m_mailLists.mail.Length; i++)
        {
            if (m_mailLists.mail[i].m_level <= GabeNewell.Instance.m_Level)
            {
                CreateMailEntry(i);
            }
        }
        if (!GabeNewell.Instance.m_MailsAreRead) Notifiy(); 
    }

    public void LoadLevelMail(int level)
    {
        for (int i = 0; i < m_mailLists.mail.Length; i++)
        {
            if (m_mailLists.mail[i].m_level<= level)
            {
                CreateMailEntry(i);
            }
        }
        Notifiy();
    }

    private void CreateMailEntry(int index)
    {
        m_contentBox = Instantiate(m_mailEntryPrefab);
        m_contentBox.transform.SetParent(m_mailViewPortContent.transform, false);

        RectTransform rt = m_contentBox.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;
        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0f);
        rt.localRotation = Quaternion.identity;

        Mail l_mail = m_contentBox.GetComponent<Mail>();
        l_mail.SetValues(
            m_mailLists.mail[index].m_senderPFPstring,
            m_mailLists.mail[index].m_senderArroba,
            m_mailLists.mail[index].m_emailContent,
            m_mailLists.mail[index].m_senderName,
            m_mailLists.mail[index].m_senderDate,
            m_MailContent,
            m_mailLists.mail[index].m_extraImage
        );

        rt.anchoredPosition = new Vector2(-105, -50 * m_loadedMails);
        if(index == 0)
        {
            l_mail.OnClick();
        }

        if (m_mailLists.mail[index].m_level < GabeNewell.Instance.m_Level)
        {
            l_mail.Opened();
        }

        m_loadedMails++;
        m_newMails++;
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
            m_newMails = 0;
            m_textUGUI.text = m_newMails.ToString();
            m_notificationGameObject.SetActive(false);
            GabeNewell.Instance.m_MailsAreRead = true;
        }
        m_clickCounter = 0;
    }

    /*void Update()
    {
        m_clickCounter += Time.deltaTime;
        if(Input.GetKeyUp(KeyCode.L))
        {
            LoadLevelMail(2);
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            LoadLevelMail(3);
        }
    }*/
}