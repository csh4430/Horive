using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Queue<GameObject> poolQueue = new Queue<GameObject>();

    public GameObject Pool(GameObject original, Transform parent)
    {
        GameObject result = null;
        if (poolQueue.Count == 0)
        {
            result = Instantiate(original, parent);
            result.name = original.name;
            result.transform.position = parent.position;
        }
        else
        {
            result = poolQueue.Dequeue();
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = parent.position;
            result.transform.SetParent(parent);
        }

        return result;
    }

    public GameObject Pool(GameObject original)
    {
        GameObject result = null;
        if (poolQueue.Count == 0)
        {
            result = Instantiate(original);
            result.name = original.name;
            result.transform.position = Vector2.zero;
        }
        else
        {
            result = poolQueue.Dequeue();
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = Vector2.zero;
            result.transform.SetParent(null);
        }

        return result;
    }


    public void DeSpawn(GameObject target)
    {
        poolQueue.Enqueue(target);
        target.SetActive(false);
    }
}
