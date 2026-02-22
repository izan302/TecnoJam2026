using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance { get; private set; }
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorClick;
    [SerializeField] private Texture2D cursorDrag;
    [SerializeField] private Texture2D cursorLoad;
    [SerializeField] private Texture2D cursorUnClickable;

    private Vector2 hotspot = Vector2.zero;

    public enum CursorImage
    {
        Normal,
        Clickable,
        Drag,
        Load,
        UnClickable
    }

    void Start()
    {
        Instance = this;
        SetInteractorCursor(CursorImage.Normal, Vector2.zero);
    }

    public void SetInteractorCursor(CursorImage _cursorImage, Vector2 _hotspot)
{
    Texture2D cursorToUse = cursorNormal;
    Vector2 finalHotspot = _hotspot;

    switch (_cursorImage)
    {
        case CursorImage.Normal:
            cursorToUse = cursorNormal;
            finalHotspot = new Vector2(7, 3);
            break;
        case CursorImage.Clickable:
            cursorToUse = cursorClick;
            finalHotspot = new Vector2(8, 2);
            break;
        case CursorImage.Drag:
            cursorToUse = cursorDrag;
            finalHotspot = new Vector2(12.5f, 12.5f);
            break;
        case CursorImage.Load:
            cursorToUse = cursorLoad; 
            finalHotspot = new Vector2(12.5f, 12.5f);
            break;
        case CursorImage.UnClickable:
            cursorToUse = cursorUnClickable;
            finalHotspot = new Vector2(12.5f, 12.5f);
            break;
    }

    if (_hotspot != Vector2.zero) finalHotspot = _hotspot;

    Cursor.SetCursor(cursorToUse, finalHotspot, CursorMode.Auto);
}
}