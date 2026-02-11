using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    [SerializeField] Piece activePiece;
    [SerializeField] Piece test;

    void Update()
    {
        if (activePiece == null)
        {
            if (Input.GetMouseButtonDown(1))
            {
                activePiece = Instantiate(test);
                activePiece.Setup(PieceData.AllPieces[0], new Vector2Int(0,0));
                //Vector2Int offset = activePiece.GetMinBounds();
                
            }
            if (InputManager.Instance.GetAttack())
            {
                TryPickUpPiece();
            }
        }
        else
        {
            HandleMovement();
            HandleRotation();
            HandlePlacement();
            SyncVisuals();
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
    void HandleMovement()
    {
        Vector2Int delta = Vector2Int.zero;
        if (Input.GetKeyDown(KeyCode.W)) delta = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S)) delta = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A)) delta = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D)) delta = Vector2Int.right;

        if (delta != Vector2Int.zero)
        {
            Vector2Int targetPos = activePiece.pivotGridPosition + delta;
            if (CanPlace(activePiece, targetPos, activePiece.rotation))
                activePiece.pivotGridPosition = targetPos;
        }
    }

    void TryRotate(int dir)
    {
        int targetRotation = activePiece.rotation + dir;
        if (CanPlace(activePiece, activePiece.pivotGridPosition, targetRotation))
            activePiece.rotation = targetRotation;
    }
    bool CanPlace(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var blockPos in piece.GetGridPositions(pivotPos, rotation))
        {
            if (blockPos.x < 0 || blockPos.x >= GameGod.instance.grid.GetWidth() ||
                blockPos.y < 0 || blockPos.y >= GameGod.instance.grid.GetHeight())
            {
                return false;
            }
            GridCell cell = GameGod.instance.grid.GetGridObject(blockPos.x, blockPos.y);
            if (cell == null || !cell.IsEmpty)
            {
                return false;
            }
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

    void TryPickUpPiece()
{
    Vector3 mouseWorldPos = InputManager.Instance.GetWorldMousePosition();
    RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);

    if (hit.collider != null)
    {
        Piece clickedPiece = hit.collider.GetComponentInParent<Piece>();
        if (clickedPiece != null)
        {
            activePiece = clickedPiece;

            foreach (var pos in activePiece.GetGridPositions())
            {
                GridCell cell = GameGod.instance.grid.GetGridObject(pos.x, pos.y);
                if (cell != null) cell.ClearPiece();
            }
        }
    }
}

    void SyncVisuals()
    {
        if (activePiece == null) return;

        float cellSize = GameGod.instance.grid.GetCellSize();
        Vector3 worldPos = GameGod.instance.grid.GetWorldPosition(
            activePiece.pivotGridPosition.x,
            activePiece.pivotGridPosition.y
        );

        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);
        activePiece.transform.position = worldPos + offset;

        activePiece.transform.rotation = Quaternion.Euler(0, 0, activePiece.rotation * -90f);
    }
}