using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("General config")]
    [SerializeField] GameObject gridParent;
    [SerializeField] GameObject cellBackgroundPrefab;
    [SerializeField] GameObject supplementaryGridParent;

    [Header("Referencias")]
    [SerializeField] Piece piecePrefab;
    [SerializeField] LevelDefinition currentLevelData;
    [SerializeField] GridCellVisual gridCellVisual;
    [SerializeField] GridCellVisual supplementaryGridCellVisual;

    [Header("Grid")]
    [SerializeField] int w;
    [SerializeField] int h;
    [SerializeField] float size;
    public Grid<GridCell> grid;
    [Header("Supplementary Grid")]
    [SerializeField] int supplementaryW;
    [SerializeField] int supplementaryH;
    [SerializeField] float supplementarySize; 
    public Grid<GridCell> supplementaryGrid;

    private void Awake()
    {
        instance = this;
        Resources.LoadAll<PieceData>("");
    }
    void Start()
    {
        //GenerateGrid
        grid = new Grid<GridCell>(w, h, size, gridParent.transform.position, (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));
        supplementaryGrid = new Grid<GridCell>(supplementaryW, supplementaryH, supplementarySize, supplementaryGridParent.transform.position, (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));
        
        gridCellVisual.Setup(grid);
        supplementaryGridCellVisual.Setup(supplementaryGrid);

        if (currentLevelData != null)
        {
            LoadLevelPieces(currentLevelData);
            //LoadStoredPieces(level);
        }
        
    }

    public GameObject GetBackgroundPrefab()
    {
        return cellBackgroundPrefab;
    }
    public Grid<GridCell> GetGrid(Grid<GridCell> _Grid)
    {
        if (_Grid == grid)
        {
            return grid;
        }
        else if (_Grid == supplementaryGrid)
        {
            return supplementaryGrid;
        }
        return null;
    }
    public GameObject GetGridParent(Grid<GridCell> grid)
    {
        if (grid == this.grid)
        {
            return gridParent;
        }
        else if (grid == this.supplementaryGrid)
        {
            return supplementaryGridParent;
        }
        return null;
    }
    void LoadLevelPieces(LevelDefinition levelData)
    {
        foreach (var entry in levelData.piecesToSpawn)
        {
            Piece newPiece = Instantiate(piecePrefab);
            newPiece.Setup(entry.pieceData, entry.position, grid, entry.rotation);

            float cellSize = grid.GetCellSize();
            Vector3 worldPos = grid.GetWorldPosition(entry.position.x, entry.position.y);

            Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
            newPiece.transform.position = worldPos + offset;

            foreach (var gridPos in newPiece.GetGridPositions())
            {
                grid.GetGridObject(gridPos.x, gridPos.y).Place(newPiece);
            }
        }
    }
    private void LoadStoredPieces(int level)
    {
        /*
        foreach (var entry in levelData.piecesToSpawn)
        {
            Piece newPiece = Instantiate(piecePrefab);
            newPiece.Setup(entry.pieceData, entry.position, supplementaryGrid, entry.rotation);

            float cellSize = supplementaryGrid.GetCellSize();
            Vector3 worldPos = supplementaryGrid.GetWorldPosition(entry.position.x, entry.position.y);

            Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
            newPiece.transform.position = worldPos + offset;

            foreach (var gridPos in newPiece.GetGridPositions())
            {
                supplementaryGrid.GetGridObject(gridPos.x, gridPos.y).Place(newPiece);
            }
        }
        */
    }
}
