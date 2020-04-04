using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSpawner : MonoBehaviour
{
    Room room;
    public GameObject obj;
    public float chance;
    private void Start()
    {
        room = transform.parent.parent.GetComponent<Room>();
        if (room.GetRoomType() == RoomType.ITEM || room.GetRoomType() == RoomType.START)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (room.IsActive())
        {
            Debug.Log(chance);
            if (Random.Range(0f,1f) <= chance)
            {
                Instantiate(obj,transform.position,Quaternion.identity,room.transform);
            }
            Destroy(gameObject);
        }
        
    }
}
