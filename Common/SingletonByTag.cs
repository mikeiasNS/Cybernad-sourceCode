using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonByTag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(gameObject.tag);
        if (gameObjects.Length > 1)
        {
            Destroy(gameObject);
        }
    }
}
