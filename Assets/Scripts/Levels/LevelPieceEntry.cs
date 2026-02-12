using System;
using UnityEngine;

[System.Serializable]
public struct LevelPieceEntry
{
    public PieceData pieceData;
    public Vector2Int position;
    [Range(0, 3)] public int rotation;
}
