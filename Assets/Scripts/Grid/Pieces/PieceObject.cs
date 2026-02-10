using System.Collections.Generic;
using UnityEngine;

public class PieceObject : MonoBehaviour
{
    private PieceObjectSO m_PieceObjectSO;
    private Vector2Int m_Origin; 
    private PieceObjectSO.Dir m_Dir;

    public static PieceObject Create(Vector3 _WorldPosition, Vector2Int _Origin, PieceObjectSO.Dir _Dir, PieceObjectSO _PieceObjectSO) 
    {
        Transform l_PieceObjectTransform = Instantiate(_PieceObjectSO.m_Prefab, _WorldPosition, Quaternion.Euler(_PieceObjectSO.GetRotationAngle(_Dir), 0, _PieceObjectSO.GetRotationAngle(_Dir)));

        PieceObject l_PieceObject = l_PieceObjectTransform.GetComponent<PieceObject>();
        l_PieceObject.m_PieceObjectSO = _PieceObjectSO;
        l_PieceObject.m_Origin = _Origin;
        l_PieceObject.m_Dir = _Dir;

        return l_PieceObject;

    }

    public List<Vector2Int> GetGridPositionList()
    {
        return m_PieceObjectSO.GetGridPositionList(m_Origin, m_Dir);
    }
    public void DestoySelf()
    {
        Destroy(gameObject);
    }
}
