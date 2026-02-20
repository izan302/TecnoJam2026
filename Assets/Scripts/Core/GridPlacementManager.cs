using NUnit.Framework;
using UnityEngine;

public class GridPlacementManager : MonoBehaviour
{
    public static GridPlacementManager instance;

    [SerializeField] Piece activePiece;

    [SerializeField] private float moveDelay = 0.15f;
    [SerializeField] private float rotationSpeed = 10f;
    private float moveTimer;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        if (GabeNewell.Instance.m_IsTutorialPlaying) return;
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

            if (activePiece != null)
            {
                if (CanPlace(activePiece, activePiece.pivotGridPosition, activePiece.rotation))
                {
                    activePiece.SetBorderColor(Color.green);
                }
                else
                {
                    activePiece.SetBorderColor(Color.red);
                }
            }
            HandlePlacement();
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
                UpdateActivePieceVisualPosition();
                moveTimer = moveDelay;
            }
        }
    }
    bool IsInsideGrid(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var blockPos in piece.GetGridPositions(pivotPos, rotation))
        {
            if (blockPos.x < 0 || blockPos.x >= LevelManager.instance.GetGrid(piece.GetGrid()).GetWidth() ||
              blockPos.y < 0 || blockPos.y >= LevelManager.instance.GetGrid(piece.GetGrid()).GetHeight())
            {
                return false;
            }
        }
        return true;
    }

    void TryRotate(int dir)
    {
        int targetRotationValue = activePiece.rotation + dir;
        if (IsInsideGrid(activePiece, activePiece.pivotGridPosition, targetRotationValue))
        {
            activePiece.rotation = targetRotationValue;
        }
    }
    bool CanPlace(Piece piece, Vector2Int pivotPos, int rotation)
    {
        foreach (var blockPos in piece.GetGridPositions(pivotPos, rotation))
        {
            if (blockPos.x < 0 || blockPos.x >= LevelManager.instance.GetGrid(piece.GetGrid()).GetWidth() ||
              blockPos.y < 0 || blockPos.y >= LevelManager.instance.GetGrid(piece.GetGrid()).GetHeight())
            {
                return false;
            }
            GridCell cell = LevelManager.instance.GetGrid(piece.GetGrid()).GetGridObject(blockPos.x, blockPos.y);
            if (cell == null || !cell.IsEmpty)
            {
                return false;
            }
        }
        return true;
    }
    void HandlePlacement()
    {
        if (InputManager.Instance.GetConfirm() && activePiece != null)
        {
            if (CanPlace(activePiece, activePiece.pivotGridPosition, activePiece.rotation))
            {
                Grid<GridCell> mainGrid = LevelManager.instance.GetGrid(activePiece.GetGrid());

                foreach (var pos in activePiece.GetGridPositions())
                {
                    mainGrid.GetGridObject(pos.x, pos.y).Place(activePiece);
                }

                activePiece.OnPieceSelect(false);
                activePiece.inInventory = false;
                UpdateActivePieceVisualPosition();

                activePiece = null;

                if (WinConditionManager.instance != null)
                    WinConditionManager.instance.CheckWinCondition();
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
            if (clickedPiece == null || clickedPiece.data.grabble == false) return;

            Grid<GridCell> currentGrid = LevelManager.instance.GetGrid(clickedPiece.GetGrid());
            foreach (var pos in clickedPiece.GetGridPositions())
            {
                GridCell cell = currentGrid.GetGridObject(pos.x, pos.y);
                if (cell != null && cell.placedPiece == clickedPiece)
                {
                    cell.ClearPiece();
                }
            }

            activePiece = clickedPiece;
            activePiece.SetGrid(LevelManager.instance.grid);
            activePiece.OnPieceSelect(true);
            activePiece.transform.SetParent(LevelManager.instance.GetGridParent(activePiece.GetGrid()).transform);

            if (activePiece.inInventory)
            {
                int centerX = LevelManager.instance.grid.GetWidth() / 2;
                int centerY = LevelManager.instance.grid.GetHeight() / 2;
                activePiece.pivotGridPosition = new Vector2Int(centerX, centerY);

                UpdateActivePieceVisualPosition();
            }

            if (WinConditionManager.instance != null)
                WinConditionManager.instance.CheckWinCondition();
        }
    }

    void UpdateActivePieceVisualPosition()
    {
        if (activePiece == null) return;

        Grid<GridCell> currentGrid = LevelManager.instance.GetGrid(activePiece.GetGrid());
        Vector3 worldPos = currentGrid.GetWorldPosition(activePiece.pivotGridPosition.x, activePiece.pivotGridPosition.y);
        float cellSize = currentGrid.GetCellSize();
        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        activePiece.transform.position = worldPos + offset;
    }

    public void ReturnPieceToSupplementaryGrid()
    {
        if (activePiece == null) return;
        foreach (var pos in activePiece.GetGridPositions())
        {
            GridCell cell = LevelManager.instance.GetGrid(activePiece.GetGrid()).GetGridObject(pos.x, pos.y);
            if (cell != null && cell.placedPiece == activePiece)
            {
                cell.ClearPiece();
            }
        }
        activePiece.SetGrid(LevelManager.instance.supplementaryGrid);
        activePiece.transform.SetParent(LevelManager.instance.GetGridParent(activePiece.GetGrid()).transform);
        activePiece.RestoreToHomeState();
        activePiece.inInventory = true;
        Grid<GridCell> suppGrid = LevelManager.instance.supplementaryGrid;
        Vector3 worldPos = suppGrid.GetWorldPosition(activePiece.pivotGridPosition.x, activePiece.pivotGridPosition.y);
        float cellSize = suppGrid.GetCellSize();
        Vector3 offset = new Vector3(cellSize * 0.5f, cellSize * 0.5f, 0);

        activePiece.transform.position = worldPos + offset;
        activePiece.OnPieceSelect(false);
        activePiece = null;
    }
}