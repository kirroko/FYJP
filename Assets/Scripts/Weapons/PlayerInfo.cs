using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] private TextMeshProUGUI healthText = null;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthText.text = health.ToString();

        if (health <= 0f)
        {
            SceneManager.LoadScene(0);
        }
    }
}
