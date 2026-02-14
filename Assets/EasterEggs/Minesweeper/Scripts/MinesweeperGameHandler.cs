using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MinesweeperGameHandler : MonoBehaviour
{
    public static MinesweeperGameHandler Instance { get; private set; }
    private Minesweeper m_Minesweeper;

    [Header("Configuración del Juego")]
    [SerializeField] private int m_Width = 10;
    [SerializeField] private int m_Height = 10;
    [SerializeField] private float m_Size = 1f;
    [SerializeField] private int m_MineCount = 10;

    [Header("Referencias")]
    [SerializeField] private MinesweeperPrefabVisual m_GridPrefabVisual;
    [SerializeField] private MinesweeperClickHandler m_ClickHandler;
    [SerializeField] private GameObject m_GameOverScreen;
    [SerializeField] private GameObject m_VictoryScreen;
    [SerializeField] private GameObject m_MenuScreen;
    [SerializeField] private GameObject m_GameScreen;
    [SerializeField] private GameObject m_MinesweeperWindow;
    [SerializeField] private TextMeshProUGUI m_Timer;
    private float m_StartTime;
    void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        m_Minesweeper = new Minesweeper(m_Width, m_Height, m_Size, transform, m_MineCount);

        if (m_GridPrefabVisual != null)
        {
            m_GridPrefabVisual.Setup(m_Minesweeper.GetGrid());
        }

        if (m_ClickHandler != null)
        {
            m_ClickHandler.m_GameLogic = m_Minesweeper;
        }
        if (m_MenuScreen != null)
        {
            m_MenuScreen.SetActive(false);
        }
        if (m_GameScreen != null)
        {
            m_GameScreen.SetActive(true);
        }
        m_StartTime = Time.time;
    }
    void Update()
    {
        float t = Time.time - m_StartTime;
        float minutes = (t / 60);
        float seconds = (t % 60);
        m_Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void GameOver()
    {
        if (m_GameOverScreen != null)
        {
            m_GameOverScreen.SetActive(true);
        }
        if (m_GameScreen != null)
        {
            m_GameScreen.SetActive(false);
        }
    }

    public void Victory()
    {
        if (m_GameScreen != null)
        {
            m_GameScreen.SetActive(false);
        }
        if (m_VictoryScreen != null)
        {
            m_VictoryScreen.SetActive(true);
        }
    }
    public void Retry()
    {
        StartGame();

        if (m_GameOverScreen != null)
        {
            m_GameOverScreen.SetActive(false);
        }
        if (m_VictoryScreen != null)
        {
            m_VictoryScreen.SetActive(false);
        }
    }

    public void OpenApp()
    {
        if (m_MinesweeperWindow != null)
        {
            m_MinesweeperWindow.SetActive(true);
        }
        GoToMenu();
    }

    public void GoToMenu()
    {
        if (m_MenuScreen != null)
        {
            m_MenuScreen.SetActive(true);
        }
        if (m_GameScreen != null)
        {
            m_GameScreen.SetActive(false);
        }
        if (m_GameOverScreen != null)
        {
            m_GameOverScreen.SetActive(false);
        }
        if (m_VictoryScreen != null)
        {
            m_VictoryScreen.SetActive(false);
        }
    }

}