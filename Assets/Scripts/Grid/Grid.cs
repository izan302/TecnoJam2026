using System;
using UnityEngine;

public class Grid<TGridCell>
{
    public event EventHandler<OnGridValueChangedEventArgs> OnGridValueChanged;
    public class OnGridValueChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    private int m_Width;
    private int m_Height;

    private float m_CellSize;

    private Vector3 m_OriginPosition;
    private TGridCell[,] m_GridArray;
    private TextMesh[,] m_DebugTextArray;

    public Grid(int _Width, int _Height, float _CellSize, Vector3 _OriginPosition, Func<Grid<TGridCell>, int, int, TGridCell> _CreateGridObject)
    {
        this.m_Width = _Width;
        this.m_Height = _Height;
        this.m_CellSize = _CellSize;
        this.m_OriginPosition = _OriginPosition;

        m_GridArray = new TGridCell[m_Width, m_Height];
        for (int x = 0; x < m_GridArray.GetLength(0); x++)
        {
            for (int y = 0; y < m_GridArray.GetLength(1); y++)
            {
                m_GridArray[x, y] = _CreateGridObject(this, x, y);
            }
        }
        
        bool l_ShowDebug = true;
        
        if (l_ShowDebug)
        {
            m_DebugTextArray = new TextMesh[m_Width, m_Height];
            for (int x = 0; x < m_GridArray.GetLength(0); x++)
            {
                for (int y = 0; y < m_GridArray.GetLength(1); y++)
                {
                    //m_DebugTextArray[x, y] = ShowTextInScreen(m_GridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(m_CellSize, m_CellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center, 0);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, m_Height), GetWorldPosition(m_Width, m_Height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(m_Width, 0), GetWorldPosition(m_Width, m_Height), Color.white, 100f);
        }
        OnGridValueChanged += (object _Sender, OnGridValueChangedEventArgs _EventArgs) =>
        {
            //m_DebugTextArray[_EventArgs.x, _EventArgs.y].text = m_GridArray[_EventArgs.x, _EventArgs.y]?.ToString();
        };
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
    public TGridCell GetGridObject(int _X, int _Y)
    {
        if (_X >= 0 && _Y >= 0 && _X < m_Width && _Y < m_Height) return m_GridArray[_X, _Y];
        return default(TGridCell);
    }
    public TGridCell GetGridObject(Vector3 _WorldPosition)
    {
        int l_X, l_Y;
        GetXYFromWorldPosition(_WorldPosition, out l_X, out l_Y);
        return GetGridObject(l_X, l_Y);
    }
    public Vector3 GetWorldPosition(int _X, int _Y)
    {
        return new Vector3(_X, _Y) * m_CellSize + m_OriginPosition;
    }
    public void GetXYFromWorldPosition(Vector3 _WorldPosition, out int x, out int y)
    {
        x = Mathf.FloorToInt((_WorldPosition - m_OriginPosition).x / m_CellSize);
        y = Mathf.FloorToInt((_WorldPosition - m_OriginPosition).y / m_CellSize);
    }

    public float GetCellSize()
    {
        return m_CellSize;
    }
    public int GetWidth()
    {
        return m_Width;
    }
    public int GetHeight()
    {
        return m_Height;
    }

    public void SetGridObject(int _X, int _Y, TGridCell _Value)
    {
        if (_X >= 0 && _Y >= 0 && _X < m_Width && _Y < m_Height)
        {
            m_GridArray[_X, _Y] = _Value;
            if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = _X, y = _Y });
        }
    }
    public void TriggerGridObjectChanged(int _X, int _Y)
    {
        if (OnGridValueChanged != null) OnGridValueChanged(this, new OnGridValueChangedEventArgs { x = _X, y = _Y });
    }

    public void SetGridObject(Vector3 _WorldPosition, TGridCell _Value)
    {
        int l_X, l_Y;
        GetXYFromWorldPosition(_WorldPosition, out l_X, out l_Y);
        SetGridObject(l_X, l_Y, _Value);
    }
}
