using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerManAnimationListener : MonoBehaviour
{
    private DaggerManAI ai;

    void Start()
    {
        ai = GetComponentInParent<DaggerManAI>();
    }

    void ThrowDagger()
    {
        ai.CreateDagger();
    }
}
