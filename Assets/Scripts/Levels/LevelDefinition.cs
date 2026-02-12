using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/New Level")]
public class LevelDefinition : ScriptableObject
{
    [Header("Piezas Iniciales")]
    public List<LevelPieceEntry> piecesToSpawn;
}