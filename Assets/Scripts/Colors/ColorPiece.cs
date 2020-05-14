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

/**
 * This class is used for the color wheel
 * 
 * The Color Wheel is made up of 3 pieces.
 * 
 * Left, Middle and Right.
 * 
 * Each of them is a Color Piece.
 * 
 * Every time the player changes color, displays the color by getting its data from this class
 */
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

        if(!choices.ContainsKey(data.GetMain))
            Debug.Log(data.GetMain);
        image.sprite = choices[data.GetMain];
    }
}
