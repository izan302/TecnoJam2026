using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Mail : MonoBehaviour
{
    [Header("Contenido Scroller")]
    [SerializeField] private Sprite m_senderPFP;
    [SerializeField] private string m_senderArroba;

    [Header("Contenido Mail")]
    [SerializeField] [Multiline] private string m_emailContent;
    [SerializeField] private string m_senderName;
    [SerializeField] private string m_senderDate;

    [Header("Objetos Contenido Scroller")]
    [SerializeField] private Image m_senderPFPImage;
    [SerializeField] private TextMeshProUGUI m_senderArrobaText;

    [Header("Objetos Contenido Mail")]
    [SerializeField] private GameObject m_contentGameObject;
    private ContentData m_contentData;

    void Start()
    {
        /*m_contentData = m_contentGameObject.GetComponent<ContentData>();
        m_contentData.m_senderName.text = this.m_senderName;
        m_contentData.m_senderDate.text = this.m_senderDate;
        m_contentData.m_emailContent.text = this.m_emailContent;
        m_contentData.m_emailContent.text = this.m_emailContent;*/
        m_senderArrobaText.text = m_senderArroba;
        m_senderPFPImage.sprite = m_senderPFP;
    }

    public void OnClick()
    {
        m_contentData = m_contentGameObject.GetComponent<ContentData>();
        m_contentData.m_senderName.text = this.m_senderName;
        m_contentData.m_senderDate.text = this.m_senderDate;
        m_contentData.m_senderPFP.sprite = this.m_senderPFP;
        m_contentData.m_emailContent.text = this.m_emailContent;
        m_contentData.m_emailContent.text = this.m_emailContent;
    }

}
