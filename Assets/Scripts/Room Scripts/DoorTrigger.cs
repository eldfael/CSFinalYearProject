using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    Room room;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {  
            if (!room.IsCompleted() && !room.IsActive())
            {
                room.ActivateRoom();
            }
        }
        

    }

    public void Start()
    {
        room = transform.parent.parent.GetComponent<Room>();
    }
}

