using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private int LevelIndex = 0;
    private bool once = false;

    private void OnTriggerStay2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            Debug.Log("Player in range");
            if (Gesture.heldDown && !once)
            {
                StartCoroutine(LevelManager.instance.LoadLevel(LevelIndex));
                once = true;
            }
        }
    }
}
