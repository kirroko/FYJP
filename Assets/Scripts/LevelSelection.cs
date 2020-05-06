using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Level level = null;
    [SerializeField] private float holdDuration = 1f;

    [SerializeField] private GameObject lockRef = null;
    [SerializeField] private GameObject frame = null;

    [SerializeField] private Sprite[] frameSprites = new Sprite[4];

    private float holdTime = 0f;
    private bool once = false;

    private Joystick abilityInput = null;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;

        holdTime = holdDuration;
        abilityInput = ObjectReferences.instance.abilityInput;
        frame.GetComponent<SpriteRenderer>().sprite = frameSprites[Mathf.Clamp(level.data.numStars - 1, 0, 2)];

        if (level.data.unlocked)
            lockRef.SetActive(false);
        else
            frame.GetComponent<SpriteRenderer>().sprite = frameSprites[3];
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if (abilityInput.IsPressed && !once && level.data.unlocked)
            {
                holdTime -= Time.deltaTime;

                if(holdTime <= 0f)
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = ObjectReferences.fixedTimeScale;
                    LevelManager.instance.StartLevel(level.levelNum - 1);
                    once = true;
                }
            }
        }
    }
}
