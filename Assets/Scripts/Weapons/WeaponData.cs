using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Weapon", order = 1)]
public class WeaponData : ScriptableObject
{
    public float reach;
    public float damage;
    public float atkInterval;
    public Sprite image;
    public bool isAOE;

    [HideInInspector] public float atkCooldown;
}
