using UnityEngine;
public class GridCell
{
    private Grid<GridCell> m_Grid;
    private int m_X;
    private int m_Y;

    private GameObject m_BackgroundInstance;
    public Piece placedPiece { get; private set; }
    public bool IsEmpty => placedPiece == null;
    public GridCell(Grid<GridCell> grid, int x, int y)
    {
        this.m_Grid = grid;
        this.m_X = x;
        this.m_Y = y;

        SpawnBackground();
    }
    public void Place(Piece piece)
    {
        placedPiece = piece;
    }
    public void ClearPiece()
    {
        placedPiece = null;
    }

    private void SpawnBackground()
    {
        GameObject prefab = GameGod.instance.GetBackgroundPrefab();
        
        Vector3 worldPos = m_Grid.GetWorldPosition(m_X, m_Y);

        float cellSize = m_Grid.GetCellSize();
        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        m_BackgroundInstance = Object.Instantiate(prefab, worldPos + offset, Quaternion.identity);
        m_BackgroundInstance.transform.name = $"Cell_{m_X}_{m_Y}";
        
        m_BackgroundInstance.transform.SetParent(GameGod.instance.GetGridParent().transform);
        m_BackgroundInstance.transform.localScale = new Vector3(cellSize, cellSize, 1f);
    }
}
