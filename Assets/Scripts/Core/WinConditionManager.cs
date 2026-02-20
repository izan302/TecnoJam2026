using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WinConditionManager : MonoBehaviour
{
    public static WinConditionManager instance;
    [SerializeField] Button SendButton;
    [Header("ErrorSprites")]
    [SerializeField] GameObject GridNotFull;
    [SerializeField] GameObject BannedPiece;
    [SerializeField] GameObject MissingGenere;
    [SerializeField] GameObject MissingItem;
    [SerializeField] GameObject MissingMechanic;
    [SerializeField] GameObject MissingStyle;

    bool showSendButton = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        SendButton.interactable = false;
        CheckWinCondition();
        }
    public void CheckWinCondition()
    {
        showSendButton = true;
        Grid<GridCell> grid = LevelManager.instance.grid;
        int width = grid.GetWidth();
        int height = grid.GetHeight();
        //grid.LogGrid();

        HashSet<Type> foundTypes = new HashSet<Type>();
        bool t = false;
        bool j = false;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                
                GridCell cell = grid.GetGridObject(x, y);
                if (cell == null || cell.IsEmpty)
                {
                    showSendButton = false;
                    j = true;
                }
                GridNotFull.SetActive(j);
                Piece piece = cell.placedPiece;

                
                if (piece != null)
                {
                    PieceData data = piece.data;
                    foreach (var prop in data.properties)
                    {
                        if (LevelManager.instance.restrictedProperties.Contains(prop))
                        {
                            showSendButton = false;
                            t = true;
                        }
                    }
                    foundTypes.Add(data.piceType);
                }
            }
        }
        BannedPiece.SetActive(t);
        GridNotFull.SetActive(j);
        foreach (Type reqType in LevelManager.instance.requiredTypes)
        {
            if (!foundTypes.Contains(reqType))
            {
                showSendButton = false;
                switch(reqType)
                {
                    case Type.Genero: MissingGenere.SetActive(true); break;
                    case Type.Mecanica: MissingMechanic.SetActive(true); break;
                    case Type.Item: MissingItem.SetActive(true); break;
                    case Type.Estilo: MissingStyle.SetActive(true); break;
                }
            }
            else
            {
                switch (reqType)
                {
                    case Type.Genero: MissingGenere.SetActive(false); break;
                    case Type.Mecanica: MissingMechanic.SetActive(false); break;
                    case Type.Item: MissingItem.SetActive(false); break;
                    case Type.Estilo: MissingStyle.SetActive(false); break;
                }
            }
        }
        SendButton.interactable = showSendButton;
    }
}