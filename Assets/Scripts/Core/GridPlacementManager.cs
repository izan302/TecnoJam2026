using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public static GridPlacementManager instance;

    [SerializeField] Piece activePiece;
    [SerializeField] Piece test;

    [SerializeField] private float moveDelay = 0.15f;
    [SerializeField] private float rotationSpeed = 10f;
    private Quaternion targetVisualRotation;
    private float moveTimer;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {

        if (activePiece == null)
        {

            if (Input.GetMouseButtonDown(0))
            {
                TryPickUpPiece();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                ReturnPieceToSupplementaryGrid();
            }
            moveTimer -= Time.deltaTime;

            HandleMovement();
            HandleRotation();
            HandlePlacement();
            //SyncVisuals();
        }
    }

    void HandleRotation()
    {
        if (activePiece == null) return;

        if (InputManager.Instance.GetLeftRotation())
            TryRotate(-1);

        if (InputManager.Instance.GetRightRotation())
            TryRotate(1);
    }
    void HandleMovement()
    {
        if (moveTimer > 0) return;

        Vector2Int delta = Vector2Int.zero;
        if (InputManager.Instance.GetUp()) delta = Vector2Int.up;
        if (InputManager.Instance.GetDown()) delta = Vector2Int.down;
        if (InputManager.Instance.GetLeft()) delta = Vector2Int.left;
        if (InputManager.Instance.GetRight()) delta = Vector2Int.right;

        if (delta != Vector2Int.zero)
        {
            Vector2Int targetPos = activePiece.pivotGridPosition + delta;

            if (IsInsideGrid(activePiece, targetPos, activePiece.rotation))
            {
                activePiece.pivotGridPosition = targetPos;
                moveTimer = moveDelay;
            }
        }
    }
    bool IsInsideGrid(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var blockPos in piece.GetGridPositions(pivotPos, rotation))
        {
            if (blockPos.x < 0 || blockPos.x >= LevelManager.instance.grid.GetWidth() ||
                blockPos.y < 0 || blockPos.y >= LevelManager.instance.grid.GetHeight())
            {
                return false;
            }
        }
        return true;
    }

    void TryRotate(int dir)
    {
        int targetRotationValue = activePiece.rotation + dir;

        if (CanPlace(activePiece, activePiece.pivotGridPosition, targetRotationValue))
        {
            activePiece.rotation = targetRotationValue;
        }
    }
    bool CanPlace(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var blockPos in piece.GetGridPositions(pivotPos, rotation))
        {
            if (blockPos.x < 0 || blockPos.x >= LevelManager.instance.grid.GetWidth() ||
                blockPos.y < 0 || blockPos.y >= LevelManager.instance.grid.GetHeight())
            {
                return false;
            }
            GridCell cell = LevelManager.instance.grid.GetGridObject(blockPos.x, blockPos.y);
            if (cell == null || !cell.IsEmpty)
            {
                return false;
            }
        }
        return true;
    }
    void HandlePlacement()
    {
        if (InputManager.Instance.GetConfirm())
        {
            if (CanPlace(activePiece, activePiece.pivotGridPosition, activePiece.rotation))
            {
                foreach (var pos in activePiece.GetGridPositions())
                {
                    LevelManager.instance.grid.GetGridObject(pos.x, pos.y).Place(activePiece);
                }
                activePiece = null;

                if (WinConditionManager.instance != null)
                {
                    WinConditionManager.instance.CheckWinCondition();
                }
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
                targetVisualRotation = activePiece.transform.rotation;

                foreach (var pos in activePiece.GetGridPositions())
                {
                    GridCell cell = LevelManager.instance.grid.GetGridObject(pos.x, pos.y);
                    if (cell != null) cell.ClearPiece();
                }
                clickedPiece.SetGrid(LevelManager.instance.grid);
                if (WinConditionManager.instance != null)
                {
                    WinConditionManager.instance.CheckWinCondition();
                }
            }
        }
    }
    /*
    void SyncVisuals()
    {
        if (activePiece == null) return;

        float cellSize = LevelManager.instance.GetGrid(activePiece.GetGrid()).GetCellSize();
        Vector3 worldPos = LevelManager.instance.GetGrid(activePiece.GetGrid()).GetWorldPosition(
            activePiece.pivotGridPosition.x,
            activePiece.pivotGridPosition.y
        );
        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        activePiece.transform.position = Vector3.Lerp(activePiece.transform.position, worldPos + offset, Time.deltaTime * rotationSpeed);

        activePiece.transform.rotation = Quaternion.Slerp(
            activePiece.transform.rotation,
            targetVisualRotation,
            Time.deltaTime * rotationSpeed
        );
    }
    */

    public void ReturnPieceToSupplementaryGrid()
    {
        activePiece.SetGrid(LevelManager.instance.supplementaryGrid);
        //SyncVisuals();
        activePiece = null;
    }
}