using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public float health = 100f;
    public float damage = 50f;

    [SerializeField] private TextMeshProUGUI healthText = null;

    public void TakeDamage(float damage)
    {
        //health -= damage;

        //healthText.text = health.ToString();

        //if (health <= 0f)
        //    SceneManager.LoadScene(0);
    }
}
