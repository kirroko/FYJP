using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject main = null;

    public void ToMain()
    {
        main.SetActive(true);
        gameObject.SetActive(false);
    }
}
