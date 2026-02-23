using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("General config")]
    [SerializeField] GameObject gridParent;
    [SerializeField] GameObject cellBackgroundPrefab;
    [SerializeField] GameObject supplementaryGridParent;

    [Header("Levels")]
    [SerializeField] LevelDefinition[] levels;
    public List<Type> requiredTypes;
    public List<Properties> restrictedProperties;

    [Header("Referencias")]
    [SerializeField] Piece piecePrefab;
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
    [Header("Scroll")]
    [SerializeField] private Scrollable scrollController;
    [SerializeField] private float visibleAreaHeight = 10f;

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

        int level = GabeNewell.Instance.m_Level;
        //int level = 1;

        if (levels != null)
        {
            DefineBanedProperties(levels[level - 1]);
            LoadLevelPieces(levels[level - 1]);
            LoadStoredPieces(level);
        }

    }

    private void DefineBanedProperties(LevelDefinition levelDefinition)
    {
        foreach (Properties p in levelDefinition.restrictedProperties)
        {
            restrictedProperties.Add(p);
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
        if (levelData.piecesToSpawn == null) return;
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
        List<PieceData> piecesToSpawn = new List<PieceData>();
        foreach (PieceData p in PieceData.AllPieces)
        {
            if (p.spawningLevel <= level && p.spawningLevel >= 0 && p.despawningLevel >= level) piecesToSpawn.Add(p);
        }

        int currentX = 0;
        int currentY = 0;
        int maxRowHeight = 0;
        int padding = 1;

        foreach (PieceData p in piecesToSpawn)
        {
            (Vector2Int min, Vector2Int max) bounds = CalculatePieceBounds(p);
            int pieceWidth = bounds.max.x - bounds.min.x + 1;
            int pieceHeight = bounds.max.y - bounds.min.y + 1;

            if (currentX + pieceWidth > supplementaryW)
            {
                currentX = 0;
                currentY += maxRowHeight + padding;
                maxRowHeight = 0;
            }

            Vector2Int spawnPos = new Vector2Int(currentX - bounds.min.x, currentY - bounds.min.y);

            Piece newPiece = Instantiate(piecePrefab);
            newPiece.transform.SetParent(supplementaryGridParent.transform);
            newPiece.Setup(p, spawnPos, supplementaryGrid);
            newPiece.SaveHomeState();
            newPiece.inInventory = true;

            // Posicionamiento inicial
            float cellSize = supplementaryGrid.GetCellSize();
            Vector3 worldPos = supplementaryGrid.GetWorldPosition(spawnPos.x, spawnPos.y);
            newPiece.transform.position = worldPos + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

            foreach (var gridPos in newPiece.GetGridPositions())
            {
                if (supplementaryGrid.GetGridObject(gridPos.x, gridPos.y) != null)
                    supplementaryGrid.GetGridObject(gridPos.x, gridPos.y).Place(newPiece);
            }

            currentX += pieceWidth + padding;
            if (pieceHeight > maxRowHeight) maxRowHeight = pieceHeight;
        }

        float totalContentHeight = supplementaryH * supplementarySize;

        if (scrollController != null)
        {
            scrollController.SetupLimits(totalContentHeight);
        }
    }
    private (Vector2Int min, Vector2Int max) CalculatePieceBounds(PieceData data)
    {
        if (data.blocks == null || data.blocks.Count == 0) return (Vector2Int.zero, Vector2Int.zero);

        Vector2Int min = data.blocks[0];
        Vector2Int max = data.blocks[0];

        foreach (var block in data.blocks)
        {
            Vector2Int pos = block - data.pivot;

            if (pos.x < min.x) min.x = pos.x;
            if (pos.y < min.y) min.y = pos.y;
            if (pos.x > max.x) max.x = pos.x;
            if (pos.y > max.y) max.y = pos.y;
        }
        return (min, max);
    }
}

