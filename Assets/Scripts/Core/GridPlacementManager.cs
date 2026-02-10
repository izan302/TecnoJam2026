using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    [SerializeField] Piece activePiece;
    [SerializeField] Piece test;

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandlePlacement();
    }
    void HandleMovement()
    {
        if (activePiece == null) return;

        Vector2Int delta = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W)) delta = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) delta = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) delta = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) delta = Vector2Int.right;

        if (delta != Vector2Int.zero)
        {
            Vector2Int newPos = activePiece.pivotGridPosition + delta;
            if (CanPlace(activePiece, newPos, activePiece.rotation))
                activePiece.pivotGridPosition = newPos;
        }
    }
    void HandleRotation()
    {
        if (activePiece == null) return;

        if (Input.GetKeyDown(KeyCode.Q))
            TryRotate(-1);

        if (Input.GetKeyDown(KeyCode.E))
            TryRotate(1);
    }

    void TryRotate(int dir)
    {
        int newRotation = activePiece.rotation + dir;
        if (CanPlace(activePiece, activePiece.pivotGridPosition, newRotation))
            activePiece.rotation = newRotation;
    }
    bool CanPlace(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var block in piece.GetGridPositions())
        {
            GridCell cell = GameGod.instance.grid.GetGridObject(block.x, block.y);
            if (cell == null || !cell.IsEmpty)
                return false;
        }
        return true;
    }
    void HandlePlacement()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (CanPlace(activePiece, activePiece.pivotGridPosition, activePiece.rotation))
            {
                foreach (var pos in activePiece.GetGridPositions())
                {
                    GameGod.instance.grid.GetGridObject(pos.x, pos.y).Place(activePiece);
                }
                activePiece = null;
            }
        }
    }


}