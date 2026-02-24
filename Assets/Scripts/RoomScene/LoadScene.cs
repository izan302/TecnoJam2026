using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScene : MonoBehaviour
{
    [SerializeField] private string m_SceneToLoad;

    public void OnClick()
    {
        SceneManager.LoadScene(m_SceneToLoad);
    }
}
