using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterImagePool : MonoBehaviour
{
    // Value
    [SerializeField]
    private int amountOfObjects = 10;

    [SerializeField]
    private GameObject afterImagePrefab = null;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    private void GrowPool()
    {
        for (int i = 0; i < amountOfObjects; i++)
        {
            GameObject instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            instanceToAdd.SetActive(false);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    public GameObject GetFromPool()
    {
        Debug.Log(availableObjects.Count);
        if (availableObjects.Count == 0)
        {
            Debug.Log("Did you just try to grow this?");
            GrowPool();
        }
        GameObject instance = availableObjects.Dequeue();
        Debug.Log("Insatce " + instance);
        Debug.Log("object " + availableObjects);
        instance.SetActive(true);
        return instance;
    }
}
