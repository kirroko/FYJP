using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxRadius = 10f;
    [SerializeField] private Transform pickUpPoint = null;
    [SerializeField] private Transform environment = null;

    private PlayerInfo info = null;

    private bool thrown = false;
    private float radius = 0f;

    private float scanCD = 0f;
    private float scanInterval = 0.1f;

    private GameObject temp = null;


    private void Start()
    {
        info = GetComponent<PlayerInfo>();
        temp = info.item;
        OffRenderer(environment);
    }

    private void Update()
    {
        if (info.item != null && Input.GetMouseButton(0) && !thrown)
        {
            thrown = true;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            GameObject item = info.item;
            OffRenderer(environment);

            item.transform.SetParent(null);
            item.AddComponent<Rigidbody>();
            item.GetComponent<Rigidbody>().AddForce(ray.direction * throwForce, ForceMode.Impulse);
        }
        //if (info.item != null && Gesture.pressed && !thrown)
        //{
        //    thrown = true;
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //    GameObject item = info.item;
        //    OffRenderer(environment);

        //    item.transform.SetParent(null);
        //    item.AddComponent<Rigidbody>();
        //    item.GetComponent<Rigidbody>().AddForce(ray.direction * throwForce, ForceMode.Impulse);
        //}

        if (Input.GetKeyDown(KeyCode.R))
        {
            info.item = temp;
            info.item.transform.SetParent(pickUpPoint);
            info.item.transform.localPosition = Vector3.zero;
            if(info.item.GetComponent<Rigidbody>() != null)
                Destroy(info.item.GetComponent<Rigidbody>());
        }
    }

    private void FixedUpdate()
    {
        if(thrown)
        {
            scanCD -= Time.fixedDeltaTime;

            if(info.item.GetComponent<Rigidbody>().velocity.Equals(Vector3.zero) && scanCD <= 0f)
            {
                scanCD = scanInterval;
                Collider[] collided = Physics.OverlapSphere(info.item.transform.position, radius);
                foreach(Collider collider in collided)
                {
                    collider.gameObject.GetComponent<MeshRenderer>().enabled = true;
                }

                radius += 1f;
                if (radius >= maxRadius)
                {
                    info.item = null;
                    radius = 1f;
                    thrown = false;
                }
            }
        }
    }

    private void OffRenderer(Transform obj)
    {
        for (int i = 0; i < obj.childCount; i++)
        {
            Transform child = obj.GetChild(i);
            child.GetComponent<MeshRenderer>().enabled = false;
            OffRenderer(child);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(temp.transform.position, radius);
    }
}
