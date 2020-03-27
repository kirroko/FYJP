using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    public WhiteColor GetCurrentColor { get { return currentColor; } }

    [SerializeField] private ColorManager colorManager = null;
    [SerializeField] private Image colorSelection = null;
    [SerializeField] private float minX = 1f;

    private WhiteColor currentColor = null;
    private bool canChoose = false;
    private bool colorChanged = false;

    private COLORS index = 0;

    private void Start()
    {
        currentColor = colorManager.colorList[COLORS.WHITE];
    }

    private void Update()
    {
        if (Gesture.heldDown)
        {
            canChoose = true;
            colorSelection.gameObject.SetActive(canChoose);
        }
        else
        {
            if(colorChanged)
                UpdateColor(index);

            colorChanged = false;
            canChoose = false;
            colorSelection.gameObject.SetActive(canChoose);
            index = 0;
        }

        if (canChoose)
        {
            if (Gesture.currentPos.x - Gesture.pressPos.x > minX)
                index = COLORS.BLUE;
            else if (Gesture.currentPos.x - Gesture.pressPos.x < -minX)
                index = COLORS.YELLOW;
            else
                index = COLORS.RED;

            //TBR
            if(index == COLORS.YELLOW)
            {
                colorSelection.color = new Color(1f, 1f, 0f);
            }
            else if (index == COLORS.RED)
            {
                colorSelection.color = new Color(1f, 0f, 0f);
            }
            else if (index == COLORS.BLUE)
            {
                colorSelection.color = new Color(0f, 0f, 1f);
            }
            colorChanged = true;
        }

        currentColor.UpdateAbility(gameObject);
    }

    private void UpdateColor(COLORS index)
    {
        foreach(WhiteColor color in colorManager.colorList.Values)
        {
            //Player is white before choosing color
            if (index == color.GetMain && currentColor.GetMain == COLORS.WHITE)
            {
                currentColor = color;
                Debug.Log(currentColor.name);
                return;
            }
            else if (IsCombination(color, currentColor.GetMain, index))
            {
                currentColor = color;
                Debug.Log(currentColor.name);
                return;
            }
        }
    }

    private bool IsChildColor(WhiteColor color, COLORS currentColor)
    {
        return currentColor == color.GetParent1 || currentColor == color.GetParent2;
    }

    private bool IsCombination(WhiteColor color, COLORS currentColor, COLORS index)
    {
        if (currentColor == index) return false;

        return IsChildColor(color, currentColor) && IsChildColor(color, index);
    }
}
