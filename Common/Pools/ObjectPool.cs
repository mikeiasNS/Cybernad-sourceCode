using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;
        
    protected Stack<GameObject> pool;

    public Action<int, int> onPoolChangeListener = null;

    protected void Awake()
    {
        pool = new Stack<GameObject>();
    }

    protected void CreateObject()
    {
        var created = Instantiate(prefab, transform);
        created.GetComponent<IPoolable>().SetPool(this);
        ReturnObject(created);
    }

    public void ReturnObject(GameObject obj)
    {
        obj.GetComponent<IPoolable>().OnEntersPool();
        pool.Push(obj);

        if(onPoolChangeListener != null)
            onPoolChangeListener.Invoke(pool.Count, pool.Count - 1);
    }

    public virtual GameObject GetObject()
    {
        var obj = pool.Pop();
        obj.GetComponent<IPoolable>().OnExitPool();

        if (onPoolChangeListener != null)
            onPoolChangeListener.Invoke(pool.Count, pool.Count + 1);

        return obj;
    }

    public int ObjectsLeft()
    {
        return pool.Count;
    }

    public virtual bool HasObject()
    {
        return pool.Count > 0;
    }
}
