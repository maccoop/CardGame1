using System.Collections.Generic;
using UnityEngine;

public class PoolingService : MonoBehaviour, Service
{
    private Dictionary<string, List<GameObject>> _poolingObjects;
    public void Init()
    {
        _poolingObjects = new();
    }

    public void Register(string key, GameObject gameObject)
    {
        if (_poolingObjects.ContainsKey(key))
        {
            throw new System.Exception("Service {0} was register!".FormatHelper(key));
        }
        var obj = Instantiate(gameObject, transform);
        obj.gameObject.SetActive(false);
        _poolingObjects.Add(key, new List<GameObject>() { obj });
    }

    public GameObject Resolve(GameObject gameObject, Transform parent = null)
    {
        string key = gameObject.name;
        if (!_poolingObjects.ContainsKey(key))
        {
            Register(key, gameObject);
        }
        var list = _poolingObjects[key];
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].activeSelf)
            {
                continue;
            }
            list[i].gameObject.SetActive(true);
            return list[i];
        }
        var result = Instantiate(list[0], parent);
        result.gameObject.SetActive(true);
        _poolingObjects[key].Add(result);
        return result;
    }

    public void Abort(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }
}
