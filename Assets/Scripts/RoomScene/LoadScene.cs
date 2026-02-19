using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    [SerializeField] private string m_SceneToLoad;

    public void OnClick()
    {
        SceneManager.LoadScene(m_SceneToLoad);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("RoomScene");
    }

    public void GoToDesktop()
    {
        SceneManager.LoadScene("JanScene");
    }

    public void GoToGameplay()
    {
        SceneManager.LoadScene("IzanScene");
    }
}
