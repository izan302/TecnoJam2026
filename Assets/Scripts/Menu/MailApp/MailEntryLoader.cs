using UnityEngine;

public class MailEntryLoader : MonoBehaviour
{
    public TextAsset m_textJSON;
    [SerializeField]public MailLists m_mailLists = new MailLists();

    public GameObject m_mailEntryPrefab;
    public GameObject m_MailContent;
    private GameObject m_contentBox;
    [System.Serializable]
    public class MailJsonData
    {
        public string m_senderPFPstring;
        public string m_senderArroba;
        public string m_emailContent;
        public string m_senderName;
        public string m_senderDate;
    }
    public class MailLists
    {
        public MailJsonData[] mail;
    }

    void Start()
    {
        m_mailLists = JsonUtility.FromJson<MailLists>(m_textJSON.text);
        for (int i = 0; i<m_mailLists.mail.Length; i++)
        {
            m_contentBox = Instantiate(m_mailEntryPrefab);
            m_contentBox.transform.SetParent(transform);
            Mail l_mail = m_contentBox.GetComponent<Mail>();
            //Sprite l_PFP = Resources.Load(m_mailLists.mail[i].m_senderPFPstring) as Sprite;
            l_mail.SetValues(/*l_PFP*/m_mailLists.mail[i].m_senderPFPstring, m_mailLists.mail[i].m_senderArroba, m_mailLists.mail[i].m_emailContent, m_mailLists.mail[i].m_senderName, m_mailLists.mail[i].m_senderDate, m_MailContent);
            RectTransform l_rectTransform = m_contentBox.GetComponent<RectTransform>();
            l_rectTransform.anchoredPosition = new Vector3(-92, -50 * i, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
