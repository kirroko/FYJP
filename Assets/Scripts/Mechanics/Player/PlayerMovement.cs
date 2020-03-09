using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 10f;

    private void Update()
    {
        Debug.Log(JoyStick.Instance.GetTouchOffset());
    }
}
