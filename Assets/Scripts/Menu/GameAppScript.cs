using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAppScript : MonoBehaviour
{
    [SerializeField] private string m_appName;
    [SerializeField] private float m_timeToDoubleClick = 1.5f;
    private float m_clickCounter;
    private void Update()
    {
        m_clickCounter += Time.deltaTime;
    }
    public void AppClick()
    {
        if (m_clickCounter < m_timeToDoubleClick)
        {
            if (GabeNewell.Instance.m_Level == 0) 
                GabeNewell.Instance.LevelUp();
            SceneManager.LoadScene(m_appName);
        }
        m_clickCounter = 0;
    }
}
