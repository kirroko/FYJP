using UnityEngine;
using UnityEngine.UI;

public class ObjectReferences : MonoBehaviour
{
    public static ObjectReferences instance = null;
    public static float fixedTimeScale = 0f;
    public Joystick joystick = null;
    public HoldButton jumpButton = null;
    public HoldButton dashButton = null;
    public RectTransform helperImage = null;
    public Image currentImage = null;

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
