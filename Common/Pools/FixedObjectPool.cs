using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedObjectPool : ObjectPool
{
    [SerializeField]
    private int amount;

    private Stack<GameObject> pool;

    protected new void Awake()
    {
        base.Awake();
        CreateAll();
    }

    private void CreateAll()
    {
        for(int i = 0; i < amount; i++)
        {
            CreateObject();
        }
    }
}
