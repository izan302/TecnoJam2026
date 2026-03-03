using Unity.VisualScripting;
using UnityEngine;

public class YourGame : MonoBehaviour
{
    [SerializeField] private GameObject m_yourGameUI;
    void Start()
    {
        m_yourGameUI.SetActive(false);
    }

    void Update()
    {
        if (GabeNewell.Instance.m_GameEnded)
        {
            m_yourGameUI.SetActive(true);
        }
    }
}
