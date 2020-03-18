using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchWorld : MonoBehaviour
{
    [SerializeField] private GameObject worldOne;
    [SerializeField] private GameObject worldTwo;

    private void Start()
    {
        worldTwo.SetActive(false);
    }

    public void Switch()
    {
        worldOne.SetActive(!worldOne.activeSelf);
        worldTwo.SetActive(!worldTwo.activeSelf);
    }
}
