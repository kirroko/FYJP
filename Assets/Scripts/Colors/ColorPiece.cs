using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Piece
{
    public COLORS color = COLORS.WHITE;
    public Sprite sprite = null;
}

public class ColorPiece : MonoBehaviour
{
    public Image Image { get { return image; } }
    public COLORS GetMain { get { return mainColor; } }

    [SerializeField] private COLORS mainColor = COLORS.WHITE;
    [SerializeField] private List<Piece> listOfPieces = new List<Piece>();

    private Dictionary<COLORS, Sprite> choices = new Dictionary<COLORS, Sprite>();
    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();

        foreach(Piece piece in listOfPieces)
        {
            choices.Add(piece.color, piece.sprite);
        }
        image.sprite = choices[COLORS.WHITE];
    }

    public void UpdateData(BaseColor data)
    {
        mainColor = data.GetMain;

        if (choices.ContainsKey(data.GetMain))
            image.sprite = choices[data.GetMain];
        else
            Debug.Log(name + ": " + data.GetMain);
        //image.color = data.Color;
    }
}
