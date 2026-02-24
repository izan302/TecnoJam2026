using UnityEngine;

public class SendButton : MonoBehaviour
{
    bool m_Clicked = false;
    public void OnClick()
    {
        if (!m_Clicked)
        {
            if (GabeNewell.Instance.m_Level() == LevelManager.instance.levels.Length)
                GabeNewell.Instance.GoToEndGame();
            else
                GabeNewell.Instance.LevelUp();
            m_Clicked = true;
        }
    }
}
