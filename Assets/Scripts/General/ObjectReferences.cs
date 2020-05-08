using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectReferences : MonoBehaviour
{
    public static ObjectReferences instance = null;
    public static float fixedTimeScale = 0f;

    [Header("Inputs")]
    public Joystick movementInput = null;
    public Joystick colorInput = null;
    public Joystick abilityInput = null;
    public HoldButton jumpButton = null;
    public HoldButton dashButton = null;

    [Header("Color Pieces")]
    public ColorPiece leftPiece = null;
    public ColorPiece centerPiece = null;
    public ColorPiece rightPiece = null;

    [Header("UI")]
    public Image colorIndicator = null;
    public TextMeshProUGUI itemCount = null;
    public TextMeshProUGUI time = null;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        fixedTimeScale = Time.fixedDeltaTime;
        instance = this;
        DontDestroyOnLoad(this);
    }
}
