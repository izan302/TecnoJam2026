using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class MinesweeperGridCell
{
    public enum MinesweeeperCellType
    {
        Empty,
        MineNumber_1, 
        MineNumber_2, 
        MineNumber_3, 
        MineNumber_4, 
        MineNumber_5, 
        MineNumber_6, 
        MineNumber_7, 
        MineNumber_8,
        Mine
    }
    private Grid<MinesweeperGridCell> m_Grid;
    private Minesweeper m_Logic;
    private int m_X;
    private int m_Y;
    private MinesweeeperCellType m_CellType;
    private bool m_IsRevealed;
    private bool m_IsFlagged;
    public MinesweeperGridCell(Grid<MinesweeperGridCell> grid, int x, int y, Minesweeper _Logic)
    {
        this.m_Grid = grid;
        this.m_X = x;
        this.m_Y = y;
        m_IsRevealed = false;
        m_Logic = _Logic;
        m_IsFlagged = false;
        m_CellType = MinesweeeperCellType.Empty;
    }
    public void OnMouseDown() 
    {
        m_Logic.RevealCell(m_X, m_Y); 
    }
    public void ToggleFlag()
    {
        m_IsFlagged = !m_IsFlagged;
        m_Grid.TriggerGridObjectChanged(m_X, m_Y);
    }
    public bool IsFlagged()
    {
        return m_IsFlagged;
    }
    public void Reveal()
    {
        m_IsRevealed = true;
        m_Grid.TriggerGridObjectChanged(m_X, m_Y);
    }
    public bool IsRevealed()
    {
        return m_IsRevealed;
    }
    public void SetCellType(MinesweeeperCellType cellType)
    {
        m_CellType = cellType;
    }
    public void SetMineCount(int _MineCount)
    {
        switch (_MineCount)
        {
            default:
            case 0:
                SetCellType(MinesweeeperCellType.Empty);
                break;
            case 1: 
                SetCellType(MinesweeeperCellType.MineNumber_1);
                break;
            case 2:
                SetCellType(MinesweeeperCellType.MineNumber_2);
                break;
            case 3:
                SetCellType(MinesweeeperCellType.MineNumber_3);
                break;
            case 4:
                SetCellType(MinesweeeperCellType.MineNumber_4);
                break;
            case 5:
                SetCellType(MinesweeeperCellType.MineNumber_5);
                break;
            case 6:
                SetCellType(MinesweeeperCellType.MineNumber_6);
                break;
            case 7:
                SetCellType(MinesweeeperCellType.MineNumber_7);
                break;
            case 8:
                SetCellType(MinesweeeperCellType.MineNumber_8);
            break;
        }
    }
    public MinesweeeperCellType GetCellType()
    {
        return m_CellType;
    }
    public int GetMineCount()
    {
        switch (m_CellType)
        {
            default:
            case MinesweeeperCellType.Empty:
                return 0;
            case MinesweeeperCellType.MineNumber_1:
                return 1;
            case MinesweeeperCellType.MineNumber_2:
                return 2;
            case MinesweeeperCellType.MineNumber_3:
                return 3;
            case MinesweeeperCellType.MineNumber_4:
                return 4;
            case MinesweeeperCellType.MineNumber_5:
                return 5;
            case MinesweeeperCellType.MineNumber_6:
                return 6;
            case MinesweeeperCellType.MineNumber_7:
                return 7;
            case MinesweeeperCellType.MineNumber_8:
                return 8;
            case MinesweeeperCellType.Mine:
                return -1;
        }
    }

    public List<MinesweeperGridCell> GetNeighbors()
    {
        List<MinesweeperGridCell> l_Neighbors = new List<MinesweeperGridCell>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;

                int checkX = m_X + i;
                int checkY = m_Y + j;

                if (checkX >= 0 && checkX < m_Grid.GetWidth() && checkY >= 0 && checkY < m_Grid.GetHeight())
                {
                    var l_Neighbor = m_Grid.GetGridObject(checkX, checkY);
                    if (l_Neighbor != null)
                    {
                        l_Neighbors.Add(l_Neighbor);
                    }
                }
            }
        }

        return l_Neighbors;
    }
    public int GetX()
    {
        return m_X;
    }
    public int GetY()
    {
        return m_Y;
    }
    public override string ToString()
    { 
        return m_CellType.ToString();
    }
}
