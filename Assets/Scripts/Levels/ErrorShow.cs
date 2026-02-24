using UnityEngine;

public class ErrorShow : MonoBehaviour
{
    public string errorCode = string.Empty;
    public void _ShowError()
    {
        PieceInfoDisplayer.instance.DisplayError(errorCode + "title", errorCode + "message");
    }
}
