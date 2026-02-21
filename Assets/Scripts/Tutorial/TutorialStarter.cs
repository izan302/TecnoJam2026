using System;
using UnityEngine;

public class TutorialStarter : MonoBehaviour
{
    [SerializeField] GameObject m_Tutorial;
    void Start()
    {
        if (GabeNewell.Instance.m_Level == 1 && !GabeNewell.Instance.m_TutorialPlayed)
        {
            m_Tutorial.SetActive(true);
        }
    }

    public void OpenDialogue(String _Text)
    {
        m_Tutorial.SetActive(true);
        m_Tutorial.GetComponent<Tutorial>().AngryText(_Text);
    }
}
