using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles Visual effects known as mirage. It's main objective is to activate gameobject at
 * a given position determined in player movement script. Creating an illusion of a mirage when dash is been called.
 * 
 * This class has object pooling optimization and is required to be at the top level (in this case, Main Menu)
 * as to not cause errors in playermovement.cs
 * 
 */
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

    /**
     * This Function is the meat of object pooling.
     * 
     * The amount is determined in the inspector on how many object should be created. No more then 15 should be enough.
     */
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
