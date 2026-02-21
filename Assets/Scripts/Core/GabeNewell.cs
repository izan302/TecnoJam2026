using UnityEngine;
using UnityEngine.SceneManagement;

public class GabeNewell : MonoBehaviour
{
    public static GabeNewell Instance {get; private set;}
    public int m_Level {get; private set;} = 1;
    public bool m_MailsAreRead {get; set;} = false;
    public bool m_IsTutorialPlaying {get; set;} = false;
    public bool m_CrtEffect {get; set;} = true;
    public bool m_MinesweeperWon {get; set;} = false;
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
        Debug.Log(m_Level);
        SceneManager.LoadScene("JanScene");
    }
}
