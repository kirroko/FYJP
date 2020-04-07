using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int LevelIndex = 0;
    [SerializeField] private float holdDuration = 1f;

    private float holdTime = 0f;
    [SerializeField]private bool once = false;

    private void Start()
    {
        holdTime = holdDuration;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            if ((Gesture.heldDown || Input.GetKey(KeyCode.E)) && !once)
            {
                holdTime -= Time.deltaTime;

                if(holdTime<= 0f)
                {
                    Time.timeScale = 1f;
                    Time.fixedDeltaTime = ObjectReferences.fixedTimeScale;
                    StartCoroutine(LevelManager.instance.LoadLevel(LevelIndex - 1));
                    once = true;
                }
            }
        }
    }
}
