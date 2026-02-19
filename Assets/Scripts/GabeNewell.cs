using UnityEngine;

public class GabeNewell : MonoBehaviour
{
    public static GabeNewell Instance {get; private set;}
    public int m_Level {get; private set;}
    public bool m_MailsAreRead {get; set;}
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void LevelUp()
    {
        m_Level++;
        m_MailsAreRead = false;
    }
}
