using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [Header("General config")]
    [SerializeField] GameObject gridParent;
    [SerializeField] GameObject cellBackgroundPrefab;

    [Header("Referencias")]
    [SerializeField] Piece piecePrefab;
    [SerializeField] LevelDefinition currentLevelData;

    [Header("Grid")]
    [SerializeField] int w;
    [SerializeField] int h;
    [SerializeField] float size;
    public Grid<GridCell> grid;

    private void Awake()
    {
        instance = this;
        Resources.LoadAll<PieceData>("");
    }
    void Start()
    {
        //GenerateGrid
        grid = new Grid<GridCell>(w, h, size, new Vector3(20, 0), (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));

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
    public GameObject GetGridParent()
    {
        return gridParent;
    }
    void LoadLevelPieces(LevelDefinition levelData)
    {
        foreach (var entry in levelData.piecesToSpawn)
        {
            Piece newPiece = Instantiate(piecePrefab);
            newPiece.Setup(entry.pieceData, entry.position, entry.rotation);

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
        
    }
}
