using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public static JoyStick Instance = null;
    private bool pressed = false;
    private int touchIndex = 0;

    private Vector2 firstTouch = new Vector2();
    private Vector2 touchOffset = new Vector2();

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(pressed)
        {
            touchOffset = Input.GetTouch(touchIndex).position / new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
            touchOffset -= firstTouch;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!pressed)
        {
            touchIndex = Input.touchCount - 1;
            firstTouch = Input.GetTouch(touchIndex).position / new Vector2(Camera.main.scaledPixelWidth, Camera.main.scaledPixelHeight);
        }

        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        touchOffset = Vector2.zero;
    }

    public Vector2 GetTouchOffset()
    {
        return touchOffset;
    }
}
