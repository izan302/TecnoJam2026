using UnityEngine;

public class GameGod : MonoBehaviour
{
    public static GameGod instance;

    [Header("Grid")]
    [SerializeField] int w;
    [SerializeField] int h;
    [SerializeField] float size;
    public Grid<GridCell> grid;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //GenerateGrid
        grid = new Grid<GridCell>(w, h, size, new Vector3(20, 0), (Grid<GridCell> g, int x, int y) => new GridCell(g, x, y));
    }
}
