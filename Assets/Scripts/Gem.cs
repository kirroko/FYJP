using UnityEngine;
using TMPro;

public class Gem : MonoBehaviour
{
    public TextMeshProUGUI coins = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int numCoins = System.Convert.ToInt32(coins.text);
        ++numCoins;
        coins.text = numCoins.ToString();
        Destroy(gameObject);
    }
}
