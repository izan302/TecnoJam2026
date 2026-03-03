using UnityEngine;
using UnityEngine.SceneManagement;

public class GabeNewell : MonoBehaviour
{
    public static GabeNewell Instance { get; private set; }
    [SerializeField] public int level = 1;
    public bool m_MailsAreRead { get; set; } = false;
    public bool m_IsTutorialPlaying { get; set; } = false;
    public bool m_CrtEffect { get; set; } = true;
    public bool m_MinesweeperWon { get; set; } = false;
    public bool m_TutorialPlayed { get; set; } = false;
    public string m_Language { get; set; } = "ES";
    public float m_TimePlayedInMinesweeper { get; set; } = 0f;
    public bool m_MaxTimeMinesweeper { get; set; } = false;
    public bool m_GameEnded { get; set; } = false;
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
    public int m_Level()
    {
        return level;
    }
    public void m_LevelUp()
    {
        level++;
    }
    void m_Level(int i)
    {
        level = i;
    }
    public void AddMinesweeperTime()
    {
        m_TimePlayedInMinesweeper += 1f;
        if (m_TimePlayedInMinesweeper >= 5f) {
            m_MaxTimeMinesweeper = true;
        }
    }
    public void LevelUp()
    {
        m_MailsAreRead = false;
        Debug.Log(level);
        GoToTransition();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("RoomScene");
    }

    public void GoToDesktop()
    {
        SceneManager.LoadScene("DesktopScene");
    }

    public void GoToTransition()
    {
        SceneManager.LoadScene("GameSentScene");
    }
    public void GoToEndGame()
    {
        SceneManager.LoadScene("EndGame");
    }

    public void LoadDesktop()
    {
        SceneManager.LoadScene("DesktopScene", LoadSceneMode.Additive);
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene("GameplayScene");
    }

    public void GoToCinematic()
    {
        SceneManager.LoadScene("CinematicScene");
    }

    public void GotToYourGame()
    {
        SceneManager.LoadScene("YourGameExe");
    }
}
