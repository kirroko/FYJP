using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rewind : MonoBehaviour
{
    //moveableobj[world[worldIndex]] to get the dictionary of moveable objects in the world
    private Dictionary<GameObject, Dictionary<GameObject, List<TransformWrapper>>> moveableObj = new Dictionary<GameObject, Dictionary<GameObject, List<TransformWrapper>>>();

    [SerializeField] private List<GameObject> worlds = new List<GameObject>();

    private float maxRecordDuration = 10f;
    private float recordDuration = 0f;

    private bool isRewinding = false;

    private void Awake()
    {
        Rigidbody2D[] allMoveable = FindObjectsOfType<Rigidbody2D>();
        moveableObj.Add(worlds[0], new Dictionary<GameObject, List<TransformWrapper>>());
        moveableObj.Add(worlds[1], new Dictionary<GameObject, List<TransformWrapper>>());

        foreach(Rigidbody2D blocks in allMoveable)
        {
            //Moveable Blocks from world 1
            if(blocks.CompareTag("Moveable1"))
            {
                List<TransformWrapper> transforms = new List<TransformWrapper>();

                TransformWrapper temp = new TransformWrapper();
                temp.position = blocks.transform.position;
                temp.rotation = blocks.transform.rotation;
                temp.localScale = blocks.transform.localScale;

                transforms.Add(temp);
                moveableObj[worlds[0]].Add(blocks.gameObject, transforms);
            }
            //Moveable Blocks from world 2
            else if (blocks.CompareTag("Moveable2"))
            {
                List<TransformWrapper> transforms = new List<TransformWrapper>();

                TransformWrapper temp = new TransformWrapper();
                temp.position = blocks.transform.position;
                temp.rotation = blocks.transform.rotation;
                temp.localScale = blocks.transform.localScale;

                transforms.Add(temp);
                moveableObj[worlds[1]].Add(blocks.gameObject, transforms);
            }
        }
    }

    private void Update()
    {
        int index = 0;
        if (!worlds[index].activeSelf)
            index = 1;

        if (isRewinding)
        {
            bool changed = false;

            foreach (KeyValuePair<GameObject, List<TransformWrapper>> pair in moveableObj[worlds[index]])
            {
                if (pair.Value.Count <= 0) continue;

                int last = pair.Value.Count - 1;

                pair.Key.transform.position = pair.Value[last].position;
                pair.Key.transform.rotation = pair.Value[last].rotation;
                pair.Key.transform.localScale = pair.Value[last].localScale;
                pair.Value.RemoveAt(last);

                changed = true;
            }
            if (!changed)
            {
                foreach (KeyValuePair<GameObject, List<TransformWrapper>> pair in moveableObj[worlds[index]])
                {
                    TransformWrapper temp = new TransformWrapper();
                    temp.position = pair.Key.transform.position;
                    temp.rotation = pair.Key.transform.rotation;
                    temp.localScale = pair.Key.transform.localScale;
                    pair.Value.Add(temp);
                }
                isRewinding = false;
                recordDuration = 0f;
            }
            return;
        }

        recordDuration += Time.deltaTime;

        //Reached max record so clear all previous recording
        if (recordDuration > maxRecordDuration)
        {
            foreach (KeyValuePair<GameObject, List<TransformWrapper>> pair in moveableObj[worlds[index]])
            {
                pair.Value.Clear();
                TransformWrapper temp = new TransformWrapper();
                temp.position = pair.Key.transform.position;
                temp.rotation = pair.Key.transform.rotation;
                temp.localScale = pair.Key.transform.localScale;
                pair.Value.Add(temp);
            }
            recordDuration = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (isRewinding) return;

        int index = 0;
        if (!worlds[index].activeSelf)
            index = 1;

        //Record the transforms
        foreach (KeyValuePair<GameObject, List<TransformWrapper>> pair in moveableObj[worlds[index]])
        {
            int last = pair.Value.Count - 1;

            if (pair.Value[pair.Value.Count - 1] != pair.Key.transform)
            {
                TransformWrapper temp = new TransformWrapper();
                temp.position = pair.Key.transform.position;
                temp.rotation = pair.Key.transform.rotation;
                temp.localScale = pair.Key.transform.localScale;
                pair.Value.Add(temp);

            }
        }
    }

    public void StartRewind()
    {
        isRewinding = true;
    }


}
