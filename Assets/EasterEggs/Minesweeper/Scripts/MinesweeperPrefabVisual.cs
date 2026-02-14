using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinesweeperPrefabVisual : MonoBehaviour
{
    public static MinesweeperPrefabVisual Instance { get; private set; }
    [SerializeField] Transform m_MinesweeperGridPrefab;
    private List<Transform> m_VisualNodeList;
    private Transform[,] m_VisualNodeArray;
    private Grid<MinesweeperGridCell> m_Grid;
    void Awake()
    {
        Instance = this;
        m_VisualNodeList = new List<Transform>();
    }
    public void Setup(Grid<MinesweeperGridCell> _grid)
    {
        m_Grid = _grid;
        m_VisualNodeArray = new Transform[m_Grid.GetWidth(), m_Grid.GetHeight()];
        for (int x = 0; x < m_Grid.GetWidth(); x++)
        {
            for (int y = 0; y < m_Grid.GetHeight(); y++)
            {
                Vector3 l_WorldPos = m_Grid.GetWorldPosition(x, y);
                float cellSize = m_Grid.GetCellSize();
                Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
                Vector3 l_GridPosition = l_WorldPos + offset;

                Transform l_VisualNode = CreateVisualNode(l_GridPosition);
                m_VisualNodeArray[x, y] = l_VisualNode;
                m_VisualNodeList.Add(l_VisualNode);
            }
        }

        HideNodeVisuals();
        UpdateVisual(m_Grid);

        m_Grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object _Sender, Grid<MinesweeperGridCell>.OnGridValueChangedEventArgs _EventArgs)
    {
        UpdateVisual(m_Grid);
    }

    private void UpdateVisual(Grid<MinesweeperGridCell> _Grid)
    {
        HideNodeVisuals();

        for (int x = 0; x < _Grid.GetWidth(); x++)
        {
            for (int y = 0; y < _Grid.GetHeight(); y++)
            {
                MinesweeperGridCell l_MineObject = _Grid.GetGridObject(x, y);

                Transform l_VisualNode = m_VisualNodeArray[x, y];
                l_VisualNode.gameObject.SetActive(true);
                SetupVisualNode(l_VisualNode, l_MineObject);
            }
        }
    }

    private void HideNodeVisuals()
    {
        foreach (Transform i_VisualNode in m_VisualNodeList)
        {
            i_VisualNode.gameObject.SetActive(false);
        }
    }

    private Transform CreateVisualNode(Vector3 _Position)
    {        
        Transform l_VisualNode = Object.Instantiate(m_MinesweeperGridPrefab, _Position, Quaternion.identity, transform);
        l_VisualNode.transform.localScale = new Vector3(m_Grid.GetCellSize(), m_Grid.GetCellSize(), 1f);
        return l_VisualNode;
    }

    private void SetupVisualNode(Transform _VisualNode, MinesweeperGridCell _MineObject)
    {
        SpriteRenderer l_MineIcon = _VisualNode.Find("Mine").GetComponent<SpriteRenderer>();
        TextMeshPro l_MineNumber = _VisualNode.Find("MineNumber").GetComponent<TextMeshPro>();
        Transform l_HiddenTransform = _VisualNode.Find("Hidden");
        SpriteRenderer l_FlagSprite = _VisualNode.Find("Flag").GetComponent<SpriteRenderer>();

        if (!_MineObject.IsRevealed())
        {
            l_HiddenTransform.gameObject.SetActive(true);
            l_MineNumber.gameObject.SetActive(false);
            l_MineIcon.gameObject.SetActive(false);
            l_FlagSprite.gameObject.SetActive(_MineObject.IsFlagged());
            return;
        }
        switch (_MineObject.GetCellType())
        {
            default:
            case MinesweeperGridCell.MinesweeeperCellType.Empty:
                l_MineNumber.gameObject.SetActive(false);
                l_MineIcon.gameObject.SetActive(false);
                break;
            case MinesweeperGridCell.MinesweeeperCellType.Mine:
                l_MineNumber.gameObject.SetActive(false);
                l_MineIcon.gameObject.SetActive(true);
                break;
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_1:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_2:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_3:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_4:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_5:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_6:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_7:
            case MinesweeperGridCell.MinesweeeperCellType.MineNumber_8:
                l_MineNumber.gameObject.SetActive(true);
                l_MineIcon.gameObject.SetActive(false);
                switch (_MineObject.GetCellType())
                {
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_1:
                        l_MineNumber.text = "1";
                        l_MineNumber.color = Color.blue;
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_2:
                        l_MineNumber.text = "2";
                        l_MineNumber.color = Color.green;
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_3:
                        l_MineNumber.text = "3";
                        l_MineNumber.color = Color.red;
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_4:
                        l_MineNumber.text = "4";
                        l_MineNumber.color = new Color(0f, 0f, 0.5f);
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_5:
                        l_MineNumber.text = "5";
                        l_MineNumber.color = new Color(0.5f, 0f, 0f);
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_6:
                        l_MineNumber.text = "6";
                        l_MineNumber.color = new Color(0f, 0.5f, 0f);
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_7:
                        l_MineNumber.text = "7";
                        l_MineNumber.color = new Color(0.5f, 0.5f, 0f);
                        break;
                    case MinesweeperGridCell.MinesweeeperCellType.MineNumber_8:
                        l_MineNumber.text = "8";
                        l_MineNumber.color = new Color(0.5f, 0f, 0.5f);
                        break;
                }
                break;
        }
        l_HiddenTransform.gameObject.SetActive(false);
        if (_MineObject.IsFlagged())
        {
            l_FlagSprite.gameObject.SetActive(true);
        }
    }
}
