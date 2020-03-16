using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private WeaponData[] weaponList = null;
    [SerializeField] private GameObject weaponHolder = null;

    private int weaponIndex = 0;
    private float dir;

    private void Start()
    {
        weaponHolder.GetComponent<SpriteRenderer>().sprite = weaponList[weaponIndex].image;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ++weaponIndex;
            if (weaponIndex >= weaponList.Length)
                weaponIndex = 0;
            weaponHolder.GetComponent<SpriteRenderer>().sprite = weaponList[weaponIndex].image;
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            --weaponIndex;
            if (weaponIndex < 0)
                weaponIndex = weaponList.Length - 1;
            weaponHolder.GetComponent<SpriteRenderer>().sprite = weaponList[weaponIndex].image;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            WeaponData currentWeapon = weaponList[weaponIndex];

            if (currentWeapon.isAOE)
            {
                Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, currentWeapon.reach);

                foreach (Collider2D hit in results)
                {
                    Ai enemyAI = hit.GetComponent<Ai>();

                    if (enemyAI != null)
                    {
                        enemyAI.TakeDamage(currentWeapon.damage);
                    }

                    AiPatrol enemyPatrol = hit.GetComponent<AiPatrol>();

                    if (enemyPatrol != null)
                    {
                        enemyPatrol.TakeDamage(currentWeapon.damage);
                    }
                }
            }
            else
            {
                RaycastHit2D[] results = Physics2D.RaycastAll(transform.position, new Vector2(dir, 0f), weaponList[weaponIndex].reach);

                foreach (RaycastHit2D hit in results)
                {
                    Ai enemyAI = hit.collider.GetComponent<Ai>();

                    if (enemyAI != null)
                    {
                        enemyAI.TakeDamage(currentWeapon.damage);
                    }

                    AiPatrol enemyPatrol = hit.collider.GetComponent<AiPatrol>();

                    if (enemyPatrol != null)
                    {
                        enemyPatrol.TakeDamage(currentWeapon.damage);
                    }
                }
            }


        }
        dir = GetComponent<SimpleMovement>().dir;
        Debug.Log("Current Weapon Index: " + weaponIndex);
        Debug.DrawLine(transform.position, transform.position + new Vector3(dir, 0f, 0f) * weaponList[weaponIndex].reach);
    }
}
