using TMPro;
using System.Collections;
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

    [Header("Animaciones App")]
    [SerializeField] private Animation m_anim;
    [SerializeField] private AnimationClip m_OpenClip;
    [SerializeField] private AnimationClip m_CloseClip;

    private bool m_IsGameRunning;
    private float m_StartTime;
    private int m_LastMinuteMark;

    void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        m_Minesweeper = new Minesweeper(m_Width, m_Height, m_Size, transform, m_MineCount);
        m_LastMinuteMark = 0;

        if (m_GridPrefabVisual != null) m_GridPrefabVisual.Setup(m_Minesweeper.GetGrid());
        if (m_ClickHandler != null) m_ClickHandler.m_GameLogic = m_Minesweeper;
        if (m_MenuScreen != null) m_MenuScreen.SetActive(false);
        if (m_GameScreen != null) m_GameScreen.SetActive(true);

        m_StartTime = Time.time;
        m_IsGameRunning = true;
    }

    void Update()
    {
        if (!m_IsGameRunning) return;

        float t = Time.time - m_StartTime;
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);

        m_Timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (minutes > m_LastMinuteMark)
        {
            m_LastMinuteMark = minutes;
            GabeNewell.Instance.AddMinesweeperTime();
        }
    }

    public void GameOver(int x, int y)
    {
        m_IsGameRunning = false;
        m_GridPrefabVisual.ExplodeCell(x, y);
        StartCoroutine(GameOverCorroutine());
    }

    IEnumerator GameOverCorroutine()
    {
        yield return new WaitForSeconds(2);
        if (m_GameOverScreen != null) m_GameOverScreen.SetActive(true);
        if (m_GameScreen != null) m_GameScreen.SetActive(false);
    }

    public void Victory()
    {
        m_IsGameRunning = false;
        if (m_GameScreen != null) m_GameScreen.SetActive(false);
        if (m_VictoryScreen != null) m_VictoryScreen.SetActive(true);
        GabeNewell.Instance.m_MinesweeperWon = true;
    }

    public void Retry()
    {
        if (m_GameOverScreen != null) m_GameOverScreen.SetActive(false);
        if (m_VictoryScreen != null) m_VictoryScreen.SetActive(false);
        StartGame();
    }

    public void OpenApp()
    {
        if (m_MinesweeperWindow != null)
        {
            m_MinesweeperWindow.SetActive(true);
            m_anim.clip = m_OpenClip;
            m_anim.Play();
        }
        GoToMenu();
    }

    public void GoToMenu()
    {
        if (m_MenuScreen != null) m_MenuScreen.SetActive(true);
        if (m_GameScreen != null) m_GameScreen.SetActive(false);
        if (m_GameOverScreen != null) m_GameOverScreen.SetActive(false);
        if (m_VictoryScreen != null) m_VictoryScreen.SetActive(false);
    }
}