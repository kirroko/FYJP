using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerInfo : MonoBehaviour
{
    public int heart = 3;
    public PAINT_COLOURS colour;
    public TextMeshProUGUI healthText = null;
    public bool onceBlack = false;
    public Vector2 dir = new Vector2();

    private float minusCD = 0f;

    private void Start()
    {
        colour = PAINT_COLOURS.WHITE;
    }

    private void Update()
    {
        if (colour == PAINT_COLOURS.BLACK)
        {
            minusCD -= Time.deltaTime;
            if (!onceBlack)
            {
                GetComponent<PlayerMovement>().moveSpeed *= 0.5f;
                onceBlack = true;
            }

            if (minusCD <= 0f)
            {
                TakeDamage(1);
                minusCD = 3f;
            }
        }
        else if(onceBlack)
        {
            onceBlack = false;
            GetComponent<PlayerMovement>().moveSpeed *= 2f;
        }
    }

    public void TakeDamage(int damage)
    {
        heart -= damage;
        if (heart <= 0)
            SceneManager.LoadScene(0);
        healthText.text = heart.ToString();
    }
}
