using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject obj;
    void Awake()
    {
        Instantiate(obj, transform.position, Quaternion.identity);
    }

    
}
