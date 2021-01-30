using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeBasedEvent : MonoBehaviour
{
    private int callCount = 0;

    [SerializeField]
    private float callRate;
    [SerializeField]
    private float delay;
    [SerializeField]
    private UnityEvent actions;
    [SerializeField]
    private int timesToCall = -1;

    
    void Start()
    {
        
        InvokeRepeating("CallAction", delay, callRate);
    }

    void CallAction()
    {
        if(callCount > timesToCall && timesToCall > 0)
        {
            enabled = false;
            return;
        }
        actions.Invoke();
        callCount++;
    }
}
