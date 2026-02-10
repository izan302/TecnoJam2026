public class GridCell
{
    private Grid<GridCell> m_Grid;
    private int m_X;
    private int m_Y;

    private Piece placedPiece;
    public bool IsEmpty => placedPiece == null;
    public GridCell(Grid<GridCell> grid, int x, int y)
    {
        this.m_Grid = grid;
        this.m_X = x;
        this.m_Y = y;
    }
    public void Place(Piece piece)
    {
        placedPiece = piece;
    }
    public void ClearPiece()
    {
        placedPiece = null;
    }
}
