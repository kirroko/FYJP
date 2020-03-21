using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealEffector : MonoBehaviour
{
    public Transform effector = null;

    void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    Vector3 pos = Input.mousePosition;
        //    Debug.Log("MousePos: " + pos + " TouchPos: " + Input.GetTouch(0).position);
        //    pos.z = 10f;
        //    Debug.Log(Camera.main.ScreenToWorldPoint(pos));
        //}


        if (Input.touchCount > 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit[] results = Physics.RaycastAll(ray);
            foreach (RaycastHit result in results)
            {
                if(result.collider.GetComponent<RevealEffector>() != null)
                {
                    GetComponent<Renderer>().sharedMaterial.SetVector("_Position", result.point);
                }
            }
            //Vector3 pos = Input.GetTouch(0).position;
            //pos.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
            //Vector3 touchPos = Camera.main.ScreenToWorldPoint(pos);
            //Debug.Log(touchPos);
            //touchPos.z = transform.position.z;
            //GetComponent<Renderer>().sharedMaterial.SetVector("_Position", touchPos);
        }
        else
            GetComponent<Renderer>().sharedMaterial.SetVector("_Position", effector.position);

    }
}
