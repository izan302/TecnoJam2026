using UnityEngine;

public class ErrorShow : MonoBehaviour
{
    public string title = string.Empty;
    public string message = string.Empty;

    public void _ShowError()
    {
        PieceInfoDisplayer.instance.DisplayError(title, message);
    }
}
