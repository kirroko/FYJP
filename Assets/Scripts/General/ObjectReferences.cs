using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReferences : MonoBehaviour
{
    public static ObjectReferences instance = null;
    public GameObject player = null;
    public Joystick joystick = null;
    public HoldButton jumpButton = null;
    public HoldButton dashButton = null;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
}
