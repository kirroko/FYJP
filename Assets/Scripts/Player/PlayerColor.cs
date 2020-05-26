using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/**
 * This class Handles the switching of colors and updating them.
 */
public class PlayerColor : MonoBehaviour
{
    public BaseColor GetCurrentColor { get { return currentColor; } }

    [Header("Reference")]
    [SerializeField] private SpriteRenderer companionSR = null;
    [SerializeField] private ColorManager colorManager = null;
    [SerializeField] private ColorAdjustments colorAdjust = null;
    public Volume vol = null;


    [Header("Color Wheel")]
    [SerializeField] private float defaultSize = 100f;
    [SerializeField] private float growSize = 150f;///< The Size the Color Piece should be when selecting

    [Header("Color Reference")]
    [SerializeField] private Color[] colorsEffects = null;
    [SerializeField] private Color[] colorsOverlay = null;

    //Image & Input References
    private ColorPiece[] colorPieces = new ColorPiece[3];
    private Joystick colorInput = null;

    private BaseColor currentColor = null;
    private BaseColor prevColor = null;
    private bool canChoose = false;
    private bool colorChanged = false;
    private bool slowDown = false;
    private int index = 0;

    private SpriteRenderer sr = null;

    private Image colorIndicator = null;

    private void Start()
    {
        colorManager = PlayerManager.instance.GetColorManager;
        currentColor = colorManager.colorList[COLORS.WHITE];
        prevColor = currentColor;
        currentColor.InitAbility(gameObject);
        

        colorInput = ObjectReferences.instance.colorInput;
        colorPieces[0] = ObjectReferences.instance.leftPiece;
        colorPieces[1] = ObjectReferences.instance.centerPiece;
        colorPieces[2] = ObjectReferences.instance.rightPiece;
        UpdateImage();

        colorIndicator = ObjectReferences.instance.colorIndicator;
        UpdateColorWheel();
        UpdateColorUI();

        // REFERENCE CODE
        vol = Camera.main.GetComponent<Volume>();
        vol.profile.TryGet(out colorAdjust);
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (vol == null)
        {
            vol = Camera.main.GetComponent<Volume>();
            vol.profile.TryGet(out colorAdjust);
        }

        //Input detected
        if(colorInput.Direction != Vector2.zero)
        {
            canChoose = true;
        }
        else
        {
            if(colorChanged)
            {
                currentColor = colorManager.colorList[colorPieces[index].GetMain];
                UpdateColorWheel();
                UpdateColorUI();
                prevColor.ExitAbility(gameObject);
                currentColor.InitAbility(gameObject);
                prevColor = currentColor;
                EventManager.instance.TriggerPlatformColorEvent(currentColor.GetMain);

                //Reset variables to default
                colorChanged = false;
                index = -1;
                ResetColorPiecesSize();

                // Change character color hue
                // sr.color = currentColor.Color; // now change color of companion to current color hue
                if (currentColor.GetMain == COLORS.PURPLE) companionSR.color = colorsOverlay[3];
                else if (currentColor.GetMain == COLORS.ORANGE) companionSR.color = colorsOverlay[1];
                else companionSR.color = currentColor.Color;

                if (!companionSR.gameObject.GetComponent<Animator>().GetBool("Hue") && currentColor.Color != Color.white)  // hasn't change to hue yet and the next color is not white shall you change to hue
                    companionSR.gameObject.GetComponent<Animator>().SetBool("Hue", true);
                else if (currentColor.Color == Color.white)                                                                // switch out of hue if the next color is white instead
                    companionSR.gameObject.GetComponent<Animator>().SetBool("Hue", false);
            }
            canChoose = false;
            ToggleVisualEffect();
        }
        if(canChoose)
        {
            //Top left Quarter of joystick
            UpdateColorWheel();
            ResetColorPiecesSize();
            if (colorInput.Direction.x < 0f && colorInput.Direction.y > 0f &&
                colorInput.Handle.anchoredPosition.magnitude >= 40f)
            {
                float dotProduct = Vector2.Dot(colorInput.Direction, Vector2.up);
                float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

                if (angle <= 30f)//right piece
                    index = 2;
                else if(angle <= 60f)//center piece
                    index = 1;
                else if(angle <= 90f)//left piece
                    index = 0;

                colorPieces[index].Image.rectTransform.sizeDelta = new Vector2(growSize, growSize);
                
                ToggleVisualEffect(index + 1);
                colorChanged = true;
            }
            else
            {
                colorChanged = false;
                colorAdjust.colorFilter.value = colorsEffects[0];
            }
        }

        ToggleSlowDown();
        currentColor.UpdateAbility(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EventManager.instance.TriggerEnemyCollisionEvent(collision, gameObject);
    }

    private void OnDestroy()
    {
        foreach(BaseColor color in colorManager.colorList.Values)
        {
            color.OnPlayerDestroyed();
        }
    }

    /**
     * This Function Updates the Color Wheel's image to show the current color
     */
    private void UpdateImage()
    {
        foreach (ColorPiece colorPiece in colorPieces)
        {
            colorPiece.Image.color = currentColor.Color;
        }
    }

    /**
    * This Function Updates the Color Wheel's as the player is selecting the colors
    */
    private void UpdateColorWheel()
    {
        if(currentColor.GetMain == COLORS.WHITE)
        {
            colorPieces[0].UpdateData(colorManager.colorList[COLORS.WHITE]);
            colorPieces[1].UpdateData(colorManager.colorList[COLORS.WHITE]);
            colorPieces[2].UpdateData(colorManager.colorList[COLORS.WHITE]);

            bool yellowLocked = colorManager.colorList[COLORS.YELLOW].IsLocked;
            bool redLocked = colorManager.colorList[COLORS.RED].IsLocked;
            bool blueLocked = colorManager.colorList[COLORS.BLUE].IsLocked;

            if(!yellowLocked) colorPieces[0].UpdateData(colorManager.colorList[COLORS.YELLOW]);

            if (!redLocked) colorPieces[1].UpdateData(colorManager.colorList[COLORS.RED]);

            if (!blueLocked) colorPieces[2].UpdateData(colorManager.colorList[COLORS.BLUE]);
        }
        else
        {
            colorPieces[0].UpdateData(colorManager.colorList[COLORS.WHITE]);
            //Player is either orange green or purple
            if (currentColor.GetParent1 != COLORS.NONE)
            {
                //Check if color is locked if yes set to current color
                if(colorManager.colorList[currentColor.GetParent1].IsLocked)
                    colorPieces[1].UpdateData(colorManager.colorList[COLORS.WHITE]);
                    //colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetParent1]);

                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParent2].IsLocked)
                    colorPieces[2].UpdateData(colorManager.colorList[COLORS.WHITE]);
                    //colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetParent2]);
            }
            else//Player is either red yellow or blue
            {
                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParentOf1].IsLocked)
                    colorPieces[1].UpdateData(colorManager.colorList[COLORS.WHITE]);
                    //colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetParentOf1]);
                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParentOf2].IsLocked)
                    colorPieces[2].UpdateData(colorManager.colorList[COLORS.WHITE]);
                    //colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetParentOf2]);
            }
        }
    }

    /**
     * This function will slow the game down when player is choosing color
     */
    private void ToggleSlowDown()
    {
        if(slowDown && !canChoose)
        {
            slowDown = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= 4f;
        }
        else if(!slowDown && canChoose)
        {
            slowDown = true;
            Time.timeScale = 0.25f;
            Time.fixedDeltaTime *= 0.25f;
        }
    }

    /**
     * This function resets all the color wheel's pieces to their orignal size
     */
    private void ResetColorPiecesSize()
    {
        foreach(ColorPiece colorPiece in colorPieces)
        {
            colorPiece.Image.rectTransform.sizeDelta = new Vector2(defaultSize, defaultSize);
        }
    }

    private void ToggleVisualEffect()
    {
        colorAdjust.colorFilter.value = colorsEffects[0];
    }
    /**
     * This function changes the color overlay when player is selecting color
     */
    private void ToggleVisualEffect(int index)
    {
        if(currentColor.GetMain == COLORS.WHITE)
            colorAdjust.colorFilter.value = colorsEffects[index];
        else
        {
            if (index == 1)
                colorAdjust.colorFilter.value = colorsEffects[index - 1];

            if(currentColor.GetParent1 != COLORS.NONE) // Player is either orange green or purple
            {
                if (index == 2)
                    colorAdjust.colorFilter.value = colorsEffects[(int)currentColor.GetParent1];
                else if (index == 3)
                    colorAdjust.colorFilter.value = colorsEffects[(int)currentColor.GetParent2];
            }
            else // Play is either red yellow or blue
            {
                if (index == 2)
                    colorAdjust.colorFilter.value = colorsEffects[(int)currentColor.GetParentOf1];
                else if (index == 3)
                    colorAdjust.colorFilter.value = colorsEffects[(int)currentColor.GetParentOf2];
            }
        }
    }

    /**
     * This function updates the color indicator.
     */
    private void UpdateColorUI()
    {
        if (currentColor.GetMain == COLORS.WHITE)
            colorIndicator.enabled = false;
        else
            colorIndicator.enabled = true;

        switch(currentColor.GetMain)
        {
            case COLORS.RED:
                colorIndicator.color = colorsOverlay[0];
                break;
            case COLORS.BLUE:
                colorIndicator.color = colorsOverlay[2];
                break;
            case COLORS.ORANGE:
                colorIndicator.color = colorsOverlay[1];
                break;
            case COLORS.PURPLE:
                colorIndicator.color = colorsOverlay[3];
                break;
            default:
                colorIndicator.color = currentColor.Color;
                // Debug.LogWarning("Something went wrong at 'UpdateColorUI'");
                break;
        }
        //colorIndicator.color = currentColor.Color;
    }
}
