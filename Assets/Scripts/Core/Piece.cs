using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Configuraci�n Visual")]
    [SerializeField] GameObject tilePrefab;

    PieceData data;
    public Vector2Int pivotGridPosition;
    public int rotation;

    public void Setup(PieceData data, Vector2Int startGridPosition)
    {
        this.data = data;
        this.pivotGridPosition = startGridPosition;
        this.rotation = 0;

        GenerateVisuals();
    }
    void GenerateVisuals()
    {
        if (data == null) return;

        float cellSize = GameGod.instance.grid.GetCellSize();
        foreach (Vector2Int blockPos in data.blocks)
        {
            Vector2Int localGridPos = blockPos - data.pivot;
            GameObject newTile = Instantiate(tilePrefab, transform);
            newTile.transform.localPosition = new Vector3(localGridPos.x * cellSize, localGridPos.y * cellSize, 0f);
            newTile.transform.localScale = new Vector3(cellSize, cellSize, 1f);
        }
    }
    public void RotatePiece(int direction)
    {
        rotation = (rotation + direction % 4 + 4) % 4;
        transform.Rotate(0, 0, direction == 1 ? -90 : 90);
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
    public List<Vector2Int> GetGridPositions()
    {
        return GetGridPositions(this.pivotGridPosition, this.rotation);
    }

    public Vector2Int GetMinBounds()
    {
        Vector2Int min = Vector2Int.zero;
        foreach (var block in data.blocks)
        {
            Vector2Int local = block - data.pivot;
            if (local.x < min.x) min.x = local.x;
            if (local.y < min.y) min.y = local.y;
        }
        return min;
    }
}