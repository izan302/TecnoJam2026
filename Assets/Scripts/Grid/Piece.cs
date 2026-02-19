using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;

public class Piece : MonoBehaviour
{
    [Header("Configuracion Visual")]
    [SerializeField] GameObject tilePrefab;

    public PieceData data { get; private set; }
    public Vector2Int pivotGridPosition;
    public int rotation;
    private Grid<GridCell> m_Grid;
    [SerializedDictionary("PieceTypes", "Color")]
    public SerializedDictionary<Type, Color> piceColor;
    [SerializeField] private float smoothSpeed = 10f;
    private Vector3 initialScale;
    private Vector2Int storedGridPosition;
    private int storedRotation;
    public bool inInventory = false;
    List<PieceTile> tiles = new List<PieceTile>();
    public void Setup(PieceData d, Vector2Int startPos, Grid<GridCell> grid, int startRotation = 0)
    {
        data = d;
        pivotGridPosition = startPos;
        rotation = startRotation;
        m_Grid = grid;

        initialScale = Vector3.one;
        transform.localScale = Vector3.one;

        GenerateVisuals();
        UpdateVisualRotation();
    }
    void Update()
    {
        var currentGrid = LevelManager.instance.GetGrid(this.GetGrid());
        if (currentGrid == null) return;

        float cellSize = currentGrid.GetCellSize();

        Vector3 cellLocalOffset = new Vector3(pivotGridPosition.x * cellSize, pivotGridPosition.y * cellSize, 0);

        Vector3 targetLocalPos = cellLocalOffset + new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);

        Quaternion targetRot = Quaternion.Euler(0, 0, rotation * -90f);
        Vector3 targetScale = initialScale * cellSize;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, Time.deltaTime * smoothSpeed);
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * smoothSpeed);
    }

    void GenerateVisuals()
    {
        if (data == null) return;

        foreach (Vector2Int blockPos in data.blocks)
        {
            Vector2Int localGridPos = blockPos - data.pivot;
            GameObject newTile = Instantiate(tilePrefab, transform);

            newTile.transform.localPosition = new Vector3(localGridPos.x, localGridPos.y, 0f);

            newTile.transform.localScale = Vector3.one;

            if (piceColor.ContainsKey(data.piceType))
            {
                PieceTile t = newTile.GetComponent<PieceTile>();
                t.baseTile.color = piceColor[data.piceType];
                t.baseTile.sprite = data.icon;
                t.selection.enabled = false;
                tiles.Add(t);
            }
        }
    }
    public void SetBorderColor(Color color)
    {
        foreach (PieceTile t in tiles)
        {
            t.selection.color = color;
        }
    }
    public Grid<GridCell> GetGrid()
    {
        return m_Grid;
    }
    public void SetGrid(Grid<GridCell> grid)
    {
        m_Grid = grid;
    }

    public void OnPieceSelect(bool selected)
    {
        PieceInfo.instance.DisplayPiece(data, piceColor[data.piceType]);
        foreach (PieceTile t in tiles)
        {
            t.selection.enabled = selected;
            t.selection.renderingLayerMask = selected ? (uint)2 : (uint)1;
            t.baseTile.renderingLayerMask = selected ? (uint)2 : (uint)1;
        }
    }

    #region Rotations
    public void RotatePiece(int direction)
    {
        rotation = (rotation + direction % 4 + 4) % 4;
        UpdateVisualRotation();
    }
    private void UpdateVisualRotation()
    {
        transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, rotation * -90f), 1f);
    }
    private Vector2Int Rotate(Vector2Int v, int rotation)
    {
        int r = (rotation % 4 + 4) % 4;
        switch (r)
        {
            case 1: return new Vector2Int(v.y, -v.x);
            case 2: return new Vector2Int(-v.x, -v.y);
            case 3: return new Vector2Int(-v.y, v.x);
            default: return v;
        }
    }
    #endregion

    public void SaveHomeState()
    {
        storedGridPosition = pivotGridPosition;
        storedRotation = rotation;
    }
    public void RestoreToHomeState()
    {
        pivotGridPosition = storedGridPosition;
        rotation = storedRotation;
        UpdateVisualRotation();
    }

    public List<Vector2Int> GetGridPositions(Vector2Int pivot, int rotation)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (Vector2Int block in data.blocks)
        {
            Vector2Int localPos = block - data.pivot;

            Vector2Int rotatedPos = Rotate(localPos, rotation);

            result.Add(pivot + rotatedPos);
        }
        return result;
    }
    public List<Vector2Int> GetGridPositions()
    {
        return GetGridPositions(this.pivotGridPosition, this.rotation);
    }
    public Vector2Int GetMinBounds()
    {
        Vector2Int min = Vector2Int.zero;
        foreach (var block in data.blocks)
        {
            min = data.pivot;
            if (block.x < min.x) min.x = block.x;
            if (block.y < min.y) min.y = block.y;
        }
        return min;
    }
}