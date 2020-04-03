﻿using UnityEngine;
using UnityEngine.UI;

public class ObjectReferences : MonoBehaviour
{
    public static ObjectReferences instance = null;
    public GameObject player = null;
    public Joystick joystick = null;
    public HoldButton jumpButton = null;
    public HoldButton dashButton = null;
    public RectTransform helperImage = null;
    public Image currentImage = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance.gameObject);
            instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (instance == null)
            instance = this;

        if(player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
