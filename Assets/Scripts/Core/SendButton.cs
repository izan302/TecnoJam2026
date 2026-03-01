using UnityEngine;

public class SendButton : MonoBehaviour
{
    bool m_Clicked = false;
    public void OnClick()
    {
        if (!m_Clicked)
        {
            GabeNewell.Instance.LevelUp();
            m_Clicked = true;
        }
    }
}
