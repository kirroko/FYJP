using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance = null;
    public bool newTurn = false;

    [SerializeField] public float gridSize = 1f;

    private void Awake()
    {
        Instance = this;
    }
}
