using UnityEngine;

public class Collectable : MonoBehaviour
{
    public bool WillRespawn { get { return willRespawn; } set { if (collected) { willRespawn = false; } } }

    private bool willRespawn = true;
    private bool collected = false;

    private Vector3 scale = Vector3.zero;

    private void Start()
    {
        scale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Level currentLevel = LevelManager.instance.CurrentLevel;
            ++currentLevel.numCollected;
            ObjectReferences.instance.itemCount.text = currentLevel.numCollected.ToString() + "/" + currentLevel.numToCollect.ToString();
            collected = true;
            transform.localScale = Vector3.zero;
        }
    }

    public void Respawn()
    {
        if (!willRespawn) return;

        transform.localScale = scale;
    }
}
