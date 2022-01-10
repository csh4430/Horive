using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private  List<GameObject> poolList = new List<GameObject>();

    public GameObject Pool(GameObject original, Transform parent)
    {
        GameObject result = null;
        if (poolList.Count == 0)
        {
            result = Instantiate(original, parent);
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = parent.position;
        }
        else
        {
            foreach (var pool in poolList)
            {
                if (pool.name == original.name)
                {
                    result = pool;
                    result.SetActive(true);
                    result.name = original.name;
                    result.transform.position = Vector2.zero;
                    poolList.Remove(result);
                    return result;
                }
            }

            result = Instantiate(original, parent);
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = parent.position;
        }

        return result;
    }

    public GameObject Pool(GameObject original)
    {
        GameObject result = null;
        if (poolList.Count == 0)
        {
            result = Instantiate(original);
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = Vector2.zero;
        }
        else
        {
            foreach(var pool in poolList)
            {
                if (pool.name == original.name)
                {
                    result = pool;
                    result.SetActive(true);
                    result.name = original.name;
                    result.transform.position = Vector2.zero;
                    result.transform.SetParent(null);
                    poolList.Remove(result);
                    return result;
                }
            }

            result = Instantiate(original);
            result.SetActive(true);
            result.name = original.name;
            result.transform.position = Vector2.zero;
        }

        return result;
    }


    public void DeSpawn(GameObject target)
    {
        poolList.Add(target);
        target.SetActive(false);
    }
}
