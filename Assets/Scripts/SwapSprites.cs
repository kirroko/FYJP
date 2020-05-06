using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapSprites : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = null;

    public void Swap(bool state)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[System.Convert.ToInt32(state)];
    }

    public void Swap(int index)
    {
        if (index >= sprites.Length)
        {
            Debug.LogError("Index is greater than num of sprites");
            return;
        }
        GetComponent<SpriteRenderer>().sprite = sprites[index];
    }
}
