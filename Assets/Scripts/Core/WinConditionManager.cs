using System.Collections.Generic;
using UnityEngine;
public class WinConditionManager : MonoBehaviour
{
    public static WinConditionManager instance;

    [Header("Level config")]
    public List<Type> requiredTypes;
    public List<Properties> restrictedProperties;

    private void Awake()
    {
        instance = this;
    }
    public void CheckWinCondition()
    {
        Grid<GridCell> grid = LevelManager.instance.grid;
        int width = grid.GetWidth();
        int height = grid.GetHeight();

        HashSet<Type> foundTypes = new HashSet<Type>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GridCell cell = grid.GetGridObject(x, y);
                if (cell == null || cell.IsEmpty)
                {
                    Debug.Log("Grid not full");
                    return;
                }

                Piece piece = cell.placedPiece;
                if (piece != null)
                {
                    PieceData data = piece.data;
                    foreach (var prop in data.properties)
                    {
                        if (restrictedProperties.Contains(prop))
                        {
                            Debug.Log($"Piece {data.m_PieceName} has baned property: {prop}.");
                            return;
                        }
                    }
                    foundTypes.Add(data.piceType);
                }
            }
        }
        foreach (Type reqType in requiredTypes)
        {
            if (!foundTypes.Contains(reqType))
            {
                Debug.Log($"Not {reqType} piece found.");
                return;
            }
        }
        OnLevelWon();
    }

    private void OnLevelWon()
    {
        Debug.Log("¡VICTORIA! Todas las condiciones se han cumplido.");
    }
}