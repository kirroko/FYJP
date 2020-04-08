using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPiece : MonoBehaviour
{
    public Image Image { get { return image; } }
    public COLORS GetMain { get { return mainColor; } }

    [SerializeField] private COLORS mainColor = COLORS.WHITE;

    private Image image = null;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void UpdateData(WhiteColor data)
    {
        mainColor = data.GetMain;
        image.color = data.Color;
    }
}
