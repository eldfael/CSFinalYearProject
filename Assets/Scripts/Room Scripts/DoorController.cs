using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    Room room;
    private void Start()
    {
        room = transform.parent.GetComponent<Room>();
    }

    private void FixedUpdate()
    {
        if (room.IsCompleted())
        {
            Destroy(gameObject);
        }
    }
}
