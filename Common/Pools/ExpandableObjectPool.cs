using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableObjectPool : ObjectPool
{
    public override GameObject GetObject()
    {
        if (pool.Count <= 0)
            CreateObject();

        return base.GetObject();
    }

    public override bool HasObject()
    {
        return true;
    }
}
