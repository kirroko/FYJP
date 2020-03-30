using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    public WhiteColor GetCurrentColor { get { return currentColor; } }
    public GameObject GetCollidedPlatform { get { return collidedPlatform; } }

    [Header("Reference")]
    [SerializeField] private ColorManager colorManager = null;
    [SerializeField] private Image colorSelection = null;
    [SerializeField] private Image currentImage = null;
    [SerializeField] private float minX = 1f;
    [SerializeField] private float angleThreshold = 120f;

    [Header("DEBUG")]
    [SerializeField] private bool CONTROL_TOGGLE = false;

    private WhiteColor currentColor = null;
    private WhiteColor prevColor = null;
    private bool canChoose = false;
    private bool colorChanged = false;
    private bool slowDown = false;

    private COLORS index = 0;

    private GameObject collidedPlatform = null;

    private void Start()
    {
        currentColor = colorManager.colorList[COLORS.WHITE];
        prevColor = currentColor;
        currentColor.InitAbility(gameObject);
    }

    private void Update()
    {
        // Swipping control
        if(CONTROL_TOGGLE)
        {
            if (Gesture.heldDown)
            {
                canChoose = true;
                colorSelection.gameObject.SetActive(canChoose);
            }
            else
            {
                if (colorChanged)
                {
                    UpdateColor(index);
                    UpdateImage();
                    prevColor.ExitAbility(gameObject);
                    currentColor.InitAbility(gameObject);
                    prevColor = currentColor;
                }

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
                if (index == COLORS.YELLOW)
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
        }
        else
        {
            // Joystick control
            if (Gesture.heldDown)
            {
                canChoose = true;
                colorSelection.gameObject.SetActive(canChoose);
            }
            else
            {
                if (colorChanged)
                {
                    UpdateColor(index);
                    UpdateImage();
                    prevColor.ExitAbility(gameObject);
                    currentColor.InitAbility(gameObject);
                    prevColor = currentColor;
                }

                colorChanged = false;
                canChoose = false;
                colorSelection.gameObject.SetActive(canChoose);
                index = 0;
            }

            if (canChoose)
            {
                Vector3 dir = (Gesture.currentPos - Gesture.pressPos).normalized;
                Debug.Log(dir);

                float angle = 0;
                if (dir.x < 0)
                {
                    angle = 360 - (Mathf.Acos(Vector2.Dot(dir, Vector2.up)) * Mathf.Rad2Deg);
                    Debug.Log("angle " + angle);
                }
                else if (dir.x > 0)
                {
                    angle = Mathf.Acos(Vector2.Dot(dir, Vector2.up)) * Mathf.Rad2Deg;
                    Debug.Log("angle " + angle);
                }
                else
                {
                    angle = Mathf.Acos(Vector2.Dot(dir, Vector2.up)) * Mathf.Rad2Deg;
                    Debug.Log("angle " + angle);
                }


                if (angle < angleThreshold)
                    index = COLORS.BLUE;
                else if (angle > angleThreshold && angle < angleThreshold * 2)
                    index = COLORS.YELLOW;
                else
                    index = COLORS.RED;

                // TBR
                if (index == COLORS.YELLOW)
                    colorSelection.color = new Color(1f, 1f, 0f);
                else if (index == COLORS.RED)
                    colorSelection.color = new Color(1f, 0f, 0f);
                else if (index == COLORS.BLUE)
                    colorSelection.color = new Color(0f, 0f, 1f);
                colorChanged = true;
            }
        }
        ToggleSlowDown();
        currentColor.UpdateAbility(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D contact = collision.GetContact(0);

        //Check if collided object is below 
        if(contact.normal.y > 0f)
        {
            collidedPlatform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collidedPlatform == null) return;

        if (collidedPlatform.GetComponent<DamagingPlatform>() != null)
            collidedPlatform.GetComponent<DamagingPlatform>().IsDamaging = false;

        if (collidedPlatform.GetComponent<MovingPlatform>() != null)
            collidedPlatform.GetComponent<MovingPlatform>().Charging = false;

        collidedPlatform = null;
    }

    private void UpdateColor(COLORS index)
    {
        foreach(WhiteColor color in colorManager.colorList.Values)
        {
            if ((index == color.GetMain && currentColor.GetMain == COLORS.WHITE)//Player is white before choosing color
                || (IsCombination(color, currentColor.GetMain, index)))//Player has a primary color alrd
            {
                currentColor = color;
                return;
            }
            else if (index == color.GetMain && color.GetMain == currentColor.GetMain)//Player is primary color and chooses the same color
            {
                currentColor = colorManager.colorList[COLORS.WHITE];
                return;
            }
            else if(index == currentColor.GetParent1)//Player select one of the parentcolor of player's current color
            {
                currentColor = colorManager.colorList[currentColor.GetParent2];
                return;
            }
            else if(index == currentColor.GetParent2)//Player select the other color of the parentcolor of player's current color
            {
                currentColor = colorManager.colorList[currentColor.GetParent1];
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

    private void UpdateImage()
    {
        switch(currentColor.GetMain)
        {
            case COLORS.BLUE:
                currentImage.color = new Color(0f, 0f, 1f);
                break;
            case COLORS.GREEN:
                currentImage.color = new Color(0f, 1f, 0f);
                break;
            case COLORS.ORANGE:
                currentImage.color = new Color(1f, 0.4f, 0f);
                break;
            case COLORS.PURPLE:
                currentImage.color = new Color(1f, 0f, 1f);
                break;
            case COLORS.RED:
                currentImage.color = new Color(1f, 0f, 0f);
                break;
            case COLORS.WHITE:
                currentImage.color = new Color(1f, 1f, 1f);
                break;
            case COLORS.YELLOW:
                currentImage.color = new Color(1f, 1f, 0f);
                break;
        }
    }

    private void ToggleSlowDown()
    {
        if(slowDown && !Gesture.heldDown)
        {
            slowDown = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= 2f;
        }
        else if(!slowDown && Gesture.heldDown)
        {
            slowDown = true;
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime *= 0.5f;
        }
    }
}
