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

    //public static PlayerAfterImagePool Instance { get; private set; }

    public static PlayerAfterImagePool Instance = null;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
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

        if (availableObjects.Count == 0)
        {
            GrowPool();
        }
        GameObject instance = availableObjects.Dequeue();

        instance.SetActive(true);
        return instance;
    }
}
