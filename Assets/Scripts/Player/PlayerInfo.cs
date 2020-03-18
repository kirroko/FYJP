using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float health = 100f;
    public float damage = 50f;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0f)
            SceneManager.LoadScene(0);
    }
}
