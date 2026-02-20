using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
    [SerializeField] GameObject m_Tutorial;
    void Start()
    {
        if (GabeNewell.Instance.m_Level == 1)
        {
            m_Tutorial.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
