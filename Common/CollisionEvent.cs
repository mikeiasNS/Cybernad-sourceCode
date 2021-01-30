using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onTriggerEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        onTriggerEnter.Invoke();
    }
}
