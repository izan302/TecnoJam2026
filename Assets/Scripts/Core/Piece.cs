using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [Header("Configuración Visual")]
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

        foreach (Vector2Int blockPos in data.blocks)
        {
            GameObject newTile = Instantiate(tilePrefab, transform);
            Vector2Int localPos = blockPos - data.pivot;

            newTile.transform.localPosition = new Vector3(localPos.x, localPos.y, 0f);
        }
    }
    public void RotatePiece(int direction)
    {
        rotation = (rotation + direction % 4 + 4) % 4;
        transform.Rotate(0, 0, direction == 1 ? -90 : 90);
    }
    public static Vector2Int Rotate(Vector2Int v, int rotation)
    {
        rotation = (rotation % 4 + 4) % 4;

        switch (rotation)
        {
            case 0: return v;
            case 1: return new Vector2Int(-v.y, v.x);
            case 2: return new Vector2Int(-v.x, -v.y);
            case 3: return new Vector2Int(v.y, -v.x);
            default: return v;
        }
    }
    public List<Vector2Int> GetGridPositions()
    {
        List<Vector2Int> result = new();
        if (data == null) return result;

        foreach (var block in data.blocks)
        {
            Vector2Int local = block - data.pivot;
            Vector2Int rotated = Rotate(local, rotation);
            Vector2Int gridPos = pivotGridPosition + rotated;
            result.Add(gridPos);
        }

        return result;
    }
}