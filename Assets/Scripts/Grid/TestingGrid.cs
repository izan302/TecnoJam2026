using System;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;

public class TestingGrid : MonoBehaviour
{
    [SerializeField] private PieceObjectSO testObject; 
    [SerializeField] private PieceObjectSO testObject2; 
    private PieceObjectSO.Dir dir = PieceObjectSO.Dir.Down;
    Grid<GridObject> m_Grid;
    void Start()
    {
        m_Grid = new Grid<GridObject>(20, 20, 10f, new Vector3(20, 0), (Grid<GridObject> g, int x, int y) => new GridObject(g, x, y));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = InputManager.Instance.GetWorldMousePosition();
            m_Grid.GetXYFromWorldPosition(position, out int x, out int y);

            List<Vector2Int> l_GridPositionList = testObject.GetGridPositionList(new Vector2Int(x, y), dir);

            bool l_CanBuild = true;
            foreach (Vector2Int i_GridPosition in l_GridPositionList)
            {
                if (!m_Grid.GetGridObject(i_GridPosition.x, i_GridPosition.y).CanBuild())
                {
                    l_CanBuild = false;
                    break;
                }
            }

            if (l_CanBuild)
            {
                Vector2Int l_RotationOffset = testObject2.GetRotationOffset(dir);
                Vector3 l_PieceObjectWorldPosition = m_Grid.GetWorldPosition(x, y) + new Vector3(l_RotationOffset.x, l_RotationOffset.y, 0) * m_Grid.GetCellSize();

                PieceObject l_PieceObject = PieceObject.Create(l_PieceObjectWorldPosition, new Vector2Int(x, y), dir, testObject2);

                foreach (Vector2Int i_GridPosition in l_GridPositionList)
                {
                    m_Grid.GetGridObject(i_GridPosition.x, i_GridPosition.y).SetPieceObject(l_PieceObject);
                }
            } else
            {
                Debug.Log("Alredy built");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GridObject l_GridObject = m_Grid.GetGridObject(InputManager.Instance.GetWorldMousePosition());
            PieceObject l_PieceObject = l_GridObject.GetPieceObject();
            if (l_PieceObject != null)
            {
                l_PieceObject.DestoySelf();
                List<Vector2Int> l_GridPositionList = l_PieceObject.GetGridPositionList();

                foreach (Vector2Int i_GridPosition in l_GridPositionList)
                {
                    m_Grid.GetGridObject(i_GridPosition.x, i_GridPosition.y).ClearPiece();

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = PieceObjectSO.GetNextDir(dir);
            Debug.Log(dir);
        }
    }
}
