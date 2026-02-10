using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pieces/Piece Data")]
public class PieceData : ScriptableObject
{
    public string m_PieceName;
    [SerializeField] Type piceType;
    [SerializeField] List<Properties> properties;

    [SerializeField] int spawningLevel = 0;
    [SerializeField] bool grabble = true;

    [Header("Shape")]
    public List<Vector2Int> blocks;
    public Vector2Int pivot;

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
