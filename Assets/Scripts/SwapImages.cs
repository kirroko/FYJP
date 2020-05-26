using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Use This Script if there are images that needs to be swapped
 */
public class SwapImages : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites = null;

    public void Swap(bool state)
    {
        GetComponent<Image>().sprite = sprites[System.Convert.ToInt32(state)];
    }

    public void Swap(int index)
    {
        if(index >= sprites.Length)
        {
            Debug.LogError("Index is greater than num of sprites");
            return;
        }
        GetComponent<Image>().sprite = sprites[index];
    }


}
