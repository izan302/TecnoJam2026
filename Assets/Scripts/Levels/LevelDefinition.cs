using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level/New Level")]
public class LevelDefinition : ScriptableObject
{
    public int maxSuplementaryGridHeight = 100;
    public List<Properties> restrictedProperties;
    [Header("Piezas Iniciales")]
    public List<LevelPieceEntry> piecesToSpawn;
}