using UnityEngine;
using UnityEngine.UI;

public class MinesweeperButton : MonoBehaviour
{
    public void OpenApp()
    {
        MinesweeperGameHandler.Instance.OpenApp();
    }

    public void StartGame()
    {
        MinesweeperGameHandler.Instance.StartGame();
    }
    public void GoToMenu()
    {
        MinesweeperGameHandler.Instance.GoToMenu();
    }

    public void Retry()
    {
        MinesweeperGameHandler.Instance.Retry();
    }
}
