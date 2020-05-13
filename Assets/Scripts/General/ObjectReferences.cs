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
    public TextMeshProUGUI numKilled = null;

    [SerializeField] private GameObject itemLogo = null;
    [SerializeField] private GameObject timeLogo = null;  
    [SerializeField] private GameObject enemyLogo = null;  
    

    [SerializeField] private GameObject pause = null;
    [SerializeField] private GameObject settings = null;

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

    private void Start()
    {
        EventManager.instance.updateHUDEvent -= TriggerUpdateHUDEvent;
        EventManager.instance.updateHUDEvent += TriggerUpdateHUDEvent;
    }

    //Pass in true if entering level, false if exiting level
    public void TriggerUpdateHUDEvent(bool state)
    {
        //Enable Item, Time, Enemies Killed UI
        itemCount.gameObject.SetActive(state);
        time.gameObject.SetActive(state);
        numKilled.gameObject.SetActive(state);

        itemLogo.gameObject.SetActive(state);
        timeLogo.gameObject.SetActive(state);
        enemyLogo.gameObject.SetActive(state);

        //Change from setting to pause btn
        settings.SetActive(!state);

        pause.SetActive(state);
    }
}
