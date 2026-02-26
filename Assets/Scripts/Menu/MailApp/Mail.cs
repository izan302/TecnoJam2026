using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    
    [Header("Contenido Scroller")]
    [SerializeField] public Sprite m_senderPFP;
    [SerializeField] public string m_senderArroba = " ";

    [Header("Contenido Mail")]
    [SerializeField] [Multiline] public string m_emailContent = " ";
    [SerializeField] public string m_senderName = " ";
    [SerializeField] public string m_senderDate = " ";
    [SerializeField] public Sprite m_extraImage;

    [Header("Objetos Contenido Scroller")]
    [SerializeField] public string m_senderPFPstring = " ";
    [SerializeField] public Image m_senderPFPImage;
    [SerializeField] private TextMeshProUGUI m_senderArrobaText;

    [Header("Objetos Contenido Mail")]
    [SerializeField] private GameObject m_contentGameObject;
    [SerializeField] private RawImage m_readedImage;
    private ContentData m_contentData;

    private MailEntryLoader m_entryLoader;
    private int m_level;
    private bool m_opened = false;
    

    public void SetValues(string senderPFP, string senderArroba, string emailContent, string senderName, string senderDate, GameObject contentGameObject, string extraImage, MailEntryLoader mel, int level)
    {
        m_senderPFPstring = senderPFP;
        m_senderPFP = Resources.Load<Sprite>(m_senderPFPstring) as Sprite;
        m_emailContent = emailContent;
        m_senderName = senderName;
        m_senderDate = senderDate;
        m_senderArroba = senderArroba;
        m_contentGameObject = contentGameObject;
        m_senderArrobaText.text = m_senderArroba;
        m_senderPFPImage.sprite = m_senderPFP;
        m_senderPFPImage.sprite = Resources.Load<Sprite>(m_senderPFPstring) as Sprite;
        m_extraImage = Resources.Load<Sprite>(extraImage) as Sprite;
        m_contentData = m_contentGameObject.GetComponent<ContentData>();
        m_entryLoader = mel;
        m_level = level;
    }

    public void OnClick()
    {
        if(m_extraImage != null)
        {
            m_contentData.m_contentImage.rectTransform.sizeDelta = new Vector2(m_extraImage.rect.size.x, m_extraImage.rect.size.y);
            m_contentData.m_contentImage.sprite = this.m_extraImage;
            m_contentData.m_contentImage.gameObject.SetActive(true);
        }
        else
        {
            m_contentData.m_contentImage.gameObject.SetActive(false);
        }

        m_contentData.m_senderName.text = this.m_senderName;
        m_contentData.m_senderDate.text = this.m_senderDate;
        m_contentData.m_senderPFP.sprite = this.m_senderPFP;
        m_contentData.m_senderPFP.GetComponent<Image>().color = Color.white;
        m_contentData.m_emailContent.text = this.m_emailContent;
        if(m_opened == false)
        {
            Opened();
        }
    }

    public void Opened()
    {
        m_readedImage.color = new Color(255, 255, 255, 0);
        if(m_level == GabeNewell.Instance.level)
        {
            m_entryLoader.OpenedNewEntry();
        }
        m_opened = true;
    }

}
