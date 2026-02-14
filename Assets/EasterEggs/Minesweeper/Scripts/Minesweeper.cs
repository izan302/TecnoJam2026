using System.Collections.Generic;
using UnityEngine;

public class Minesweeper
{
    [SerializeField] int m_Width;
    [SerializeField] int m_Height;
    [SerializeField] float m_Size;
    Grid<MinesweeperGridCell> m_Grid;
    [SerializeField] Transform m_GridParent;
    public Minesweeper(int width, int height, float size, Transform gridParent, int _mineCount = 10)
    {
        this.m_Width = width;
        this.m_Height = height;
        this.m_Size = size;
        this.m_GridParent = gridParent;
        m_Grid = new Grid<MinesweeperGridCell>(m_Width, m_Height, m_Size, new Vector3(m_GridParent.position.x, m_GridParent.position.y), (Grid<MinesweeperGridCell> g, int x, int y) => new MinesweeperGridCell(g, x, y, this));

        int i_MinePlaced = 0;
        while (i_MinePlaced < _mineCount)
        {
            int x = Random.Range(0, m_Width);
            int y = Random.Range(0, m_Height);

            MinesweeperGridCell l_Cell = m_Grid.GetGridObject(x, y);
            if (l_Cell != null && l_Cell.GetCellType() != MinesweeperGridCell.MinesweeeperCellType.Mine)
            {
                l_Cell.SetCellType(MinesweeperGridCell.MinesweeeperCellType.Mine);
                i_MinePlaced++;
            }
        }

        for (int x = 0; x < m_Width; x++)
        {
            for (int y = 0; y < m_Height; y++)
            {
                MinesweeperGridCell l_Cell = m_Grid.GetGridObject(x, y);
                if (l_Cell.GetCellType() == MinesweeperGridCell.MinesweeeperCellType.Empty)
                {
                    List<MinesweeperGridCell> l_NeighbourList = l_Cell.GetNeighbors();

                    foreach (MinesweeperGridCell i_Neighbor in l_NeighbourList)
                    {
                        if (i_Neighbor.GetCellType() == MinesweeperGridCell.MinesweeeperCellType.Mine)
                        {
                            l_Cell.SetMineCount(l_Cell.GetMineCount() + 1);
                        }
                    }
                }

            }
        }
        m_Grid.OnGridValueChanged += Grid_OnGridValueChanged;
    }

    void Grid_OnGridValueChanged(object _Sender, Grid<MinesweeperGridCell>.OnGridValueChangedEventArgs _EventArgs)
    {
        if (CheckVictory())
        {
            MinesweeperGameHandler.Instance.Victory();
        }
    }
    bool CheckVictory()
    {
        for (int x = 0; x < m_Grid.GetWidth(); x++)
        {
            for (int y = 0; y < m_Grid.GetHeight(); y++)
            {
                MinesweeperGridCell l_Cell = m_Grid.GetGridObject(x, y);
                if (l_Cell != null && !l_Cell.IsRevealed() && l_Cell.GetCellType() != MinesweeperGridCell.MinesweeeperCellType.Mine)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public Grid<MinesweeperGridCell> GetGrid()
    {
        return m_Grid;
    }

    public void RevealCell(int x, int y)
    {
        MinesweeperGridCell.MinesweeeperCellType l_CellType = Reveal(x, y);
        if (l_CellType == MinesweeperGridCell.MinesweeeperCellType.Mine)
        {
            MinesweeperGameHandler.Instance.GameOver();
        }
    }

    public void HandleClickFromWindow(Vector3 worldPosition, bool _isRightClick)
    {
        if (m_Grid == null)
        {
            return;
        }

        MinesweeperGridCell l_Cell = m_Grid.GetGridObject(worldPosition);

        if (l_Cell != null)
        {
            if (_isRightClick)
            {
                if (!l_Cell.IsRevealed()) 
                {
                    l_Cell.ToggleFlag();
                    return;
                }
            }
            if (l_Cell.GetCellType() != MinesweeperGridCell.MinesweeeperCellType.Mine && l_Cell.GetCellType() != MinesweeperGridCell.MinesweeeperCellType.Empty)
            {
                List<MinesweeperGridCell> l_Neighbors = l_Cell.GetNeighbors();
                int l_FlaggedCells = 0;
                foreach (MinesweeperGridCell i_Neighbor in l_Neighbors)
                {
                    if (i_Neighbor.IsFlagged())
                    {
                        l_FlaggedCells++;
                    }
                }
                if (l_Cell.GetMineCount() == l_FlaggedCells)
                {
                    foreach (MinesweeperGridCell i_Neighbor in l_Neighbors)
                    {
                        if (!i_Neighbor.IsFlagged() && !i_Neighbor.IsRevealed())
                        {
                            RevealCell(i_Neighbor.GetX(), i_Neighbor.GetY());
                        }
                    }
                }
            }
            if (l_Cell.IsFlagged() || l_Cell.IsRevealed())
            {
                return;
            }
            RevealCell(l_Cell.GetX(), l_Cell.GetY());
        }
    }

    public MinesweeperGridCell.MinesweeeperCellType Reveal(int x, int y)
    {
        MinesweeperGridCell l_Cell = m_Grid.GetGridObject(x, y);

        if (l_Cell == null || l_Cell.IsRevealed() || l_Cell.IsFlagged())
        {
            return l_Cell != null ? l_Cell.GetCellType() : MinesweeperGridCell.MinesweeeperCellType.Empty;
        }

        l_Cell.Reveal();

        if (l_Cell.GetCellType() == MinesweeperGridCell.MinesweeeperCellType.Empty)
        {
            foreach (MinesweeperGridCell i_Neighbor in l_Cell.GetNeighbors())
            {
                if (!i_Neighbor.IsRevealed())
                {
                    Reveal(i_Neighbor.GetX(), i_Neighbor.GetY());
                }
            }
        }

        return l_Cell.GetCellType();
    }
}
