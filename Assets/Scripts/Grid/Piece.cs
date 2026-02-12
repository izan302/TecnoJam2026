using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class Piece : MonoBehaviour
{
    [Header("Configuracion Visual")]
    [SerializeField] GameObject tilePrefab;

    public PieceData data { get; private set; }
    public Vector2Int pivotGridPosition;
    public int rotation;

    [SerializedDictionary("PieceTypes", "Color")]
    public SerializedDictionary<Type, Color> piceColor;

    public void Setup(PieceData d, Vector2Int startPos, int startRotation = 0)
    {
        data = d;
        pivotGridPosition = startPos;
        rotation = startRotation; 

        GenerateVisuals();

        UpdateVisualRotation();
    }
    void GenerateVisuals()
    {
        if (data == null) return;
        Debug.Log($"Piece {data.m_PieceName} genrated");
        float cellSize = LevelManager.instance.grid.GetCellSize();
        foreach (Vector2Int blockPos in data.blocks)
        {
            Vector2Int localGridPos = blockPos - data.pivot;
            GameObject newTile = Instantiate(tilePrefab, transform);
            newTile.transform.localPosition = new Vector3(localGridPos.x * cellSize, localGridPos.y * cellSize, 0f);
            newTile.transform.localScale = new Vector3(cellSize, cellSize, 1f);
            if (piceColor[data.piceType] == null) return;
            newTile.GetComponent<SpriteRenderer>().color = piceColor[data.piceType];
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
        transform.rotation = Quaternion.Euler(0, 0, rotation * -90f);
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