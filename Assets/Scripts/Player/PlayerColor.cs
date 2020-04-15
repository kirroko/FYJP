using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerColor : MonoBehaviour
{
    public WhiteColor GetCurrentColor { get { return currentColor; } }
    public GameObject GetCollidedPlatform { get { return collidedPlatform; } }
    
    [Header("Reference")]
    [SerializeField] private ColorManager colorManager = null;
    [SerializeField] private ColorAdjustments colorAdjust = null;
    public Volume vol = null;

    [Header("Color Wheel")]
    [SerializeField] private float defaultSize = 100f;
    [SerializeField] private float growSize = 150f;

    [Header("Color Reference")]
    [SerializeField] private Color[] colorsEffects;

    //Image & Input References
    private ColorPiece[] colorPieces = new ColorPiece[3];
    private Joystick colorInput = null;

    private WhiteColor currentColor = null;
    private WhiteColor prevColor = null;
    private bool canChoose = false;
    private bool colorChanged = false;
    private bool slowDown = false;
    private int index = 0;

    private GameObject collidedPlatform = null;

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

        // REFERENCE CODE
        vol = Camera.main.GetComponent<Volume>();
        vol.profile.TryGet(out colorAdjust);
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
                UpdateImage();
                prevColor.ExitAbility(gameObject);
                currentColor.InitAbility(gameObject);
                prevColor = currentColor;
                EventManager.instance.TriggerPlatformColorEvent(currentColor.GetMain);

                //Reset variables to default
                colorChanged = false;
                index = -1;
                ResetColorPiecesSize();
            }
            canChoose = false;
            ToggleVisualEffect();
        }
        if(canChoose)
        {
            //Top left Quarter of joystick
            UpdateColorWheel();
            if (colorInput.Direction.x < 0f && colorInput.Direction.y > 0f)
            {
                float dotProduct = Vector2.Dot(colorInput.Direction, Vector2.up);
                float angle = Mathf.Acos(dotProduct) * Mathf.Rad2Deg;

                ResetColorPiecesSize();

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
        }

        ToggleSlowDown();
        currentColor.UpdateAbility(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EventManager.instance.TriggerEnemyCollisionEvent(collision, gameObject);

        if (collision.gameObject == null || collision.contactCount == 0) return;

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

    private void OnDestroy()
    {
        foreach(WhiteColor color in colorManager.colorList.Values)
        {
            color.OnPlayerDestroyed();
        }
    }

    private void UpdateImage()
    {
        foreach (ColorPiece colorPiece in colorPieces)
        {
            colorPiece.Image.color = currentColor.Color;
        }
    }

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

            Debug.Log("redLocked: " + redLocked);

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
                Debug.Log("Color Choice (!= None): " + currentColor.GetParent1 + " " + currentColor.GetParent2);
                //Check if color is locked if yes set to current color
                if(colorManager.colorList[currentColor.GetParent1].IsLocked)
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetParent1]);

                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParent2].IsLocked)
                    colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[2].UpdateData(colorManager.colorList[currentColor.GetParent2]);
            }
            else//Player is either red yellow or blue
            {
                Debug.Log("Color Choice (== None): " + currentColor.GetParentOf1 + " " + currentColor.GetParentOf2);
                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParentOf1].IsLocked)
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetParentOf1]);
                //Check if color is locked if yes set to current color
                if (colorManager.colorList[currentColor.GetParentOf2].IsLocked)
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetMain]);
                else
                    colorPieces[1].UpdateData(colorManager.colorList[currentColor.GetParentOf2]);
            }
        }
    }

    private void ToggleSlowDown()
    {
        if(slowDown && !canChoose)
        {
            slowDown = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= 2f;
        }
        else if(!slowDown && canChoose)
        {
            slowDown = true;
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime *= 0.5f;
        }
    }

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
}
