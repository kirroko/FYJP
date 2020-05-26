using System.Collections;
using UnityEngine;

/**
 * This is the script to attach to the gameobject that contains the painting in level selection screen
 */
[RequireComponent(typeof(Collider2D))]
public class LevelSelection : MonoBehaviour
{
    [SerializeField] private Level level = null;
    [SerializeField] private float holdDuration = 1f;///< Duration to hold before it counts as entering the level

    [SerializeField] private GameObject lockRef = null;
    [SerializeField] private GameObject frame = null;

    [SerializeField] private Sprite[] frameSprites = new Sprite[4];

    private float holdTime = 0f;
    private bool once = false;

    private HoldButton triggerButton = null;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;

        holdTime = holdDuration;
        triggerButton = ObjectReferences.instance.jumpButton;
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
            if (triggerButton.pressed && !once/* && level.data.unlocked*/)
            {
                holdTime -= Time.deltaTime;
                if(holdTime <= 0f)
                {
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    player.GetComponent<Animator>().SetTrigger("Enter");
                    player.GetComponent<PlayerMovement>().ForceGravityToZero();
                    //GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Enter");
                    //GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>
                    StartCoroutine(DelayEnter(0.4f,player));
                    once = true;
                    //Time.timeScale = 1f;
                    //Time.fixedDeltaTime = ObjectReferences.fixedTimeScale;
                    //LevelManager.instance.StartLevel(level.levelNum - 1);
                    //LevelManager.instance.CurrentLevel.fullSprite = GetComponent<SpriteRenderer>().sprite;
                    //once = true;
                }
            }
        }
    }

    /**
     * This Delay is used so that the animation can play finish before the player enter the game
     */
    private IEnumerator DelayEnter(float time, GameObject player)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;
        Time.fixedDeltaTime = ObjectReferences.fixedTimeScale;
        LevelManager.instance.StartLevel(level.levelNum - 1);
        LevelManager.instance.CurrentLevel.fullSprite = GetComponent<SpriteRenderer>().sprite;
        player.GetComponent<PlayerMovement>().ResetGravityToDefault();
    }
}
