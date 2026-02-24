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
        pieceName.text = LocalizationManager.instance.GetText("piece_name_"+piece.name);
        pieceType.text = LocalizationManager.instance.GetText("ui_type") + ": " + LocalizationManager.instance.GetText("piece_type_"+piece.piceType.ToString());
        pieceType.color = c;

        for (int i = 0; i < propertie.Length; i++)
        {
            if (piece.properties.Count > i)
            {
                propertie[i].enabled = true;

                Properties prp = piece.properties[i];
                propertie[i].text = "• " + LocalizationManager.instance.GetText("prp_" +prp.ToString());

                propertie[i].color = LevelManager.instance.restrictedProperties.Contains(prp) ? bannedColor : goodColor;
            }
            else
            {
                propertie[i].enabled = false;
            }
        }
    }
    public void DisplayError(string Title_Key, string Message_Key)
    {
        pieceName.text = LocalizationManager.instance.GetText(Title_Key);
        pieceType.text = LocalizationManager.instance.GetText(Message_Key);
        pieceType.color = bannedColor;

        for (int i = 0; i < propertie.Length; i++)
        {
            propertie[i].enabled = false;
        }
    }
}
