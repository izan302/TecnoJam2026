using TMPro;
using UnityEngine;
using static UnityEngine.Audio.ProcessorInstance;

public class PieceInfoDisplayer : MonoBehaviour
{
    public static PieceInfoDisplayer instance;
    [SerializeField] TextMeshProUGUI pieceName;
    [SerializeField] TextMeshProUGUI pieceType;
    [SerializeField] TextMeshProUGUI[] propertie;

    [Header("Colors")]
    [SerializeField] Color goodColor;
    [SerializeField] Color bannedColor;

    private void Awake()
    {
        instance = this;
    }

    public void DisplayPiece(PieceData piece, Color c)
    {
        pieceName.text = piece.name;
        pieceType.text = "Type: " + piece.piceType.ToString();
        pieceType.color = c;

        for (int i = 0; i < propertie.Length; i++)
        {
            if (piece.properties.Count > i)
            {
                propertie[i].enabled = true;

                Properties prp = piece.properties[i];
                propertie[i].text = "·" + prp;

                propertie[i].color = LevelManager.instance.restrictedProperties.Contains(prp) ? bannedColor : goodColor;
            }
            else
            {
                propertie[i].enabled = false;
            }
        }
    }
    public void DisplayError(string titulo, string info)
    {
        pieceName.text = titulo;
        pieceType.text = info;
        pieceType.color = bannedColor;

        for (int i = 0; i < propertie.Length; i++)
        {
            propertie[i].enabled = false;
        }
    }
}
