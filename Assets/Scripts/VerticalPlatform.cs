using UnityEngine;

[RequireComponent(typeof(PlatformEffector2D))]
public class VerticalPlatform : MonoBehaviour
{
    [SerializeField] private Joystick input = null;
    [SerializeField] private HoldButton jump = null;
    [SerializeField] private HoldButton dash = null;

    private PlatformEffector2D effector;
    public float waitTime;

    // Start is called before the first frame update
    private void Start()
    {
        GetComponent<Collider2D>().usedByEffector = true;
        effector = GetComponent<PlatformEffector2D>();
        input = ObjectReferences.instance.movementInput;
        jump = ObjectReferences.instance.jumpButton;
        dash = ObjectReferences.instance.dashButton;
    }

    // Update is called once per frame
    private void Update()
    {
        if (input.Vertical < -0.2f) // drop down
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0.5f;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }

        if (jump.tap || dash.tap) // when jumping rest the effector
            effector.rotationalOffset = 0;
    }
}