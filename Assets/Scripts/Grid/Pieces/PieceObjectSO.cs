using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu()]
public class PieceObjectSO : ScriptableObject
{
    public string m_PieceName;
    public Transform m_Prefab;
    public Transform m_Visual;
    public int m_Width;
    public int m_Height;
    public enum Dir
    {
        Down, 
        Up,
        Left,
        Right
    }

    public int GetRotationAngle(Dir _Dir)
    {
        switch (_Dir)
        {
            default:
            case Dir.Down: return 0;
            case Dir.Left: return 90;
            case Dir.Up: return 180;
            case Dir.Right: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir _Dir)
    {
        switch (_Dir)
        {
            default:
            case Dir.Down: return new Vector2Int(0, 0);
            case Dir.Left: return new Vector2Int(0,m_Width);
            case Dir.Up: return  new Vector2Int(m_Width, m_Height);
            case Dir.Right: return  new Vector2Int(m_Height, 0);
        }
    }
    public static Dir GetNextDir(Dir _Dir)
    {
        switch (_Dir)
        {
            default:
            case Dir.Down: return Dir.Left;
            case Dir.Left: return Dir.Up;
            case Dir.Up: return Dir.Right;
            case Dir.Right: return Dir.Down;
        }        
    }

    public List<Vector2Int> GetGridPositionList(Vector2Int _Offset, Dir _Dir) {
        List<Vector2Int> l_GridPositionList = new List<Vector2Int>();
        switch (_Dir)
        {
            default:
            case Dir.Down:
            case Dir.Up:
                for (int x = 0; x < m_Width; x++)
                {
                    for(int y = 0; y < m_Height; y++)
                    {
                        l_GridPositionList.Add(_Offset + new Vector2Int(x, y));
                    }
                }
            break;
            case Dir.Left: 
            case Dir.Right: 
                for (int x = 0; x < m_Height; x++)
                {
                    for (int y = 0; y < m_Width; y++)
                    {
                        l_GridPositionList.Add(_Offset + new Vector2Int(x, y));
                    }
                }
            break;
        }
        return l_GridPositionList;
    }
}
