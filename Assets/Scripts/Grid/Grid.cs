using UnityEngine;

public class Grid
{
    private int m_Width;
    private int m_Height;
    private float m_CellSize;
    private Vector3 m_OriginPosition;
    private int[,] m_GridArray;
    private TextMesh[,] m_DebugTextArray;

    public Grid(int _Width, int _Height, float _CellSize, Vector3 _OriginPosition)
    {
        this.m_Width = _Width;
        this.m_Height = _Height;
        this.m_CellSize = _CellSize;
        this.m_OriginPosition = _OriginPosition;

        m_GridArray = new int[m_Width, m_Height];
        m_DebugTextArray = new TextMesh[m_Width, m_Height];

        for (int x = 0; x < m_GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_GridArray.GetLength(1); y++)
            {
                m_DebugTextArray[x, y] = ShowTextInScreen(m_GridArray[x, y].ToString(), null, GetWorldPosition(x, y) + new Vector3(m_CellSize, m_CellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }
        Debug.DrawLine(GetWorldPosition(0, m_Height), GetWorldPosition(m_Width, m_Height), Color.white, 100f);
        Debug.DrawLine(GetWorldPosition(m_Width, 0), GetWorldPosition(m_Width, m_Height), Color.white, 100f);


    }

    protected TextMesh ShowTextInScreen(string _Text, Transform _Parent, Vector3 _LocalPosition, int _FontSize, Color _Color, TextAnchor _TextAnchor, TextAlignment _TextAlingment, int _SortingOrder)
    {
        if (_Color == null) _Color = Color.white;

        GameObject l_GameObject = new GameObject("Text", typeof(TextMesh));
        Transform l_TextTransform = l_GameObject.transform;
        l_TextTransform.SetParent(_Parent, false);
        l_TextTransform.localPosition = _LocalPosition;

        TextMesh l_TextMesh = l_GameObject.GetComponent<TextMesh>();
        l_TextMesh.anchor = _TextAnchor;
        l_TextMesh.alignment = _TextAlingment;
        l_TextMesh.text = _Text;
        l_TextMesh.fontSize = _FontSize;
        l_TextMesh.color = _Color;
        l_TextMesh.GetComponent<MeshRenderer>().sortingOrder = _SortingOrder;
        return l_TextMesh;
    }
    public int GetValue(int _X, int _Y)
    {
        if (_X >= 0 && _Y >= 0 && _X < m_Width && _Y < m_Height) return m_GridArray[_X, _Y];
        else return -1;        
    }
    public int GetValue(Vector3 _WorldPosition) 
    {
        int l_X, l_Y;
        GetXYFromWorldPosition(_WorldPosition, out l_X, out l_Y);
        return GetValue(l_X, l_Y);  
    }
    private Vector3 GetWorldPosition(int _X, int _Y)
    {
        return new Vector3(_X, _Y) * m_CellSize + m_OriginPosition;
    }
    private void GetXYFromWorldPosition(Vector3 _WorldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((_WorldPosition - m_OriginPosition).x / m_CellSize);
        y = Mathf.FloorToInt((_WorldPosition - m_OriginPosition).y / m_CellSize);
    }

    public void SetValue(int _X, int _Y, int _Value)
    {
        if (_X >= 0 && _Y >= 0 && _X < m_Width && _Y < m_Height)
        {
            m_GridArray[_X, _Y] = _Value;
            m_DebugTextArray[_X, _Y].text = m_GridArray[_X, _Y].ToString(); 
        }
    }

    public void SetValue(Vector3 _WorldPosition, int _Value)
    {
        int l_X, l_Y;
        GetXYFromWorldPosition(_WorldPosition, out l_X, out l_Y);
        SetValue(l_X, l_Y, _Value);
    }
}
