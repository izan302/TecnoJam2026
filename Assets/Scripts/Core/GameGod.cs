using System;
using System.Collections.Generic;
using UnityEngine;

public class GameGod : MonoBehaviour
{
    public static GameGod instance;

    [Header("Grid")]
    [SerializeField] int w;
    [SerializeField] int h;
    [SerializeField] float size;
    [SerializeField] GameObject gridParent;
    [SerializeField] GameObject cellBackgroundPrefab;

    public Grid<GridCell> grid;

    private void Awake()
    {
        instance = this;
        Resources.LoadAll<PieceData>("");
    }
    void Start()
    {
        //GenerateGrid
        grid = new Grid<GridCell>(w, h, size, new Vector3(20, 0), (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));
    }

    public GameObject GetBackgroundPrefab()
    {
        return cellBackgroundPrefab;
    }
    public GameObject GetGridParent()
    {
        return gridParent;
    }
    void LoadLevelPieces()
    {

    }
    private void LoadStoredPieces(int level)
    {
        
    }
}
