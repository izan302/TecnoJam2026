using System.Collections.Generic;
using UnityEngine;

public class GridCellVisual : MonoBehaviour
{
    public static GridCellVisual Instance { get; private set; }
    [SerializeField] Transform m_GridCellVisualPrefab;
    [SerializeField] Transform m_OtherGridCellVisualPrefab;
    private Transform[,] m_VisualNodeArray;
    private List<Transform> m_VisualNodeList;
    private Grid<GridCell> m_Grid;
    void Awake()
    {
        Instance = this;
    }

    public void Setup(Grid<GridCell> _grid)
    {
        m_Grid = _grid;
        m_VisualNodeArray = new Transform[m_Grid.GetWidth(), m_Grid.GetHeight()];
        m_VisualNodeList = new List<Transform>();

        for (int x = 0; x < m_Grid.GetWidth(); x++)
        {
            for (int y = 0; y < m_Grid.GetHeight(); y++)
            {
                Vector3 l_WorldPos = m_Grid.GetWorldPosition(x, y);
                float cellSize = m_Grid.GetCellSize();
                Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
                Vector3 l_GridPosition = l_WorldPos + offset;

                Transform l_PrefabToInstantiate = ((x + y) % 2 == 0) ? m_GridCellVisualPrefab : m_OtherGridCellVisualPrefab;

                Transform l_VisualNode = CreateVisualNode(l_PrefabToInstantiate, l_GridPosition);
                m_VisualNodeArray[x, y] = l_VisualNode;
                m_VisualNodeList.Add(l_VisualNode);
            }
        }

        UpdateVisual(m_Grid);

        m_Grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object _Sender, Grid<GridCell>.OnGridValueChangedEventArgs _EventArgs)
    {
        UpdateVisual(m_Grid);
    }

    private void UpdateVisual(Grid<GridCell> _Grid)
    {
        for (int x = 0; x < _Grid.GetWidth(); x++)
        {
            for (int y = 0; y < _Grid.GetHeight(); y++)
            {
                GridCell l_GridCell = _Grid.GetGridObject(x, y);

                Transform l_VisualNode = m_VisualNodeArray[x, y];
                l_VisualNode.gameObject.SetActive(true);
            }
        }
    }
    private Transform CreateVisualNode(Transform _Prefab, Vector3 _Position)
    {         
        Transform l_VisualNode = Object.Instantiate(_Prefab, _Position, Quaternion.identity, transform);
        l_VisualNode.transform.localScale = new Vector3(m_Grid.GetCellSize(), m_Grid.GetCellSize(), 1f);
        return l_VisualNode;
    }
}
