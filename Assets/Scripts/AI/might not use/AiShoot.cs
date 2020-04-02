using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiShoot : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
   

    public AiPatrol AiDirection;
    
    // Start is called before the first frame update
    void Start()
    {
        AiDirection = FindObjectOfType<AiPatrol>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
    }
}
