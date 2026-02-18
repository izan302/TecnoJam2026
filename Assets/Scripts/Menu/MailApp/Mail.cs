using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
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

    [Header("Objetos Contenido Scroller")]
    [SerializeField] public string m_senderPFPstring = " ";
    [SerializeField] public Image m_senderPFPImage;
    [SerializeField] private TextMeshProUGUI m_senderArrobaText;

    [Header("Objetos Contenido Mail")]
    [SerializeField] private GameObject m_contentGameObject;
    private ContentData m_contentData;

    public void SetValues(string senderPFP, string senderArroba, string emailContent, string senderName, string senderDate, GameObject contentGameObject)
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
    }

    public void OnClick()
    {
        m_contentData = m_contentGameObject.GetComponent<ContentData>();
        m_contentData.m_senderName.text = this.m_senderName;
        m_contentData.m_senderDate.text = this.m_senderDate;
        m_contentData.m_senderPFP.sprite = this.m_senderPFP;
        m_contentData.m_senderPFP.GetComponent<Image>().color = Color.white;
        m_contentData.m_emailContent.text = this.m_emailContent;

    }
}
