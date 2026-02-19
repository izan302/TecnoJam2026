using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pieces/Piece Data")]
public class PieceData : ScriptableObject
{
    public string m_PieceName;
    public Type piceType;
    public List<Properties> properties = new List<Properties>();
    public Sprite icon;

    public int spawningLevel = 0;
    public bool grabble = true;

    [Header("Shape")]
    public List<Vector2Int> blocks;
    [NonSerialized]public Vector2Int pivot = new Vector2Int(0,0);

    public static readonly List<PieceData> AllPieces = new List<PieceData>();

    private void OnEnable()
    {
        if (!AllPieces.Contains(this))
            AllPieces.Add(this);
    }

    private void OnDisable()
    {
        AllPieces.Remove(this);
    }
}
