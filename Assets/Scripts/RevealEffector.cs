using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealEffector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] results = Physics.RaycastAll(ray);
            foreach (RaycastHit result in results)
            {
                if (result.collider.GetComponent<RevealEffector>() != null)
                {
                    GetComponent<Renderer>().sharedMaterial.SetVector("_Position", result.point);
                }
            }
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial.SetVector("_Position", new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));
        }


        //if (Input.touchCount > 0)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit[] results = Physics.RaycastAll(ray);
        //    foreach (RaycastHit result in results)
        //    {
        //        if(result.collider.GetComponent<RevealEffector>() != null)
        //        {
        //            GetComponent<Renderer>().sharedMaterial.SetVector("_Position", result.point);
        //        }
        //    }
        //    //Vector3 pos = Input.GetTouch(0).position;
        //    //pos.z = Mathf.Abs(transform.position.z - Camera.main.transform.position.z);
        //    //Vector3 touchPos = Camera.main.ScreenToWorldPoint(pos);
        //    //Debug.Log(touchPos);
        //    //touchPos.z = transform.position.z;
        //    //GetComponent<Renderer>().sharedMaterial.SetVector("_Position", touchPos);
        //}
        //else
        //    GetComponent<Renderer>().sharedMaterial.SetVector("_Position", effector.position);

    }
}
