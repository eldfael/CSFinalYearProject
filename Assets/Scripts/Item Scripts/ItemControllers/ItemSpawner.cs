using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    Room room;
    PlayerController playerController;
    public List<GameObject> itemList;

    float totalWeight;
    private void Start()
    {
        room = transform.parent.parent.GetComponent<Room>();
        if (room == null)
        {
            room = transform.parent.parent.parent.GetComponent<Room>();
            
        }
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        totalWeight = 0;
        foreach (GameObject item in itemList)
        {
            totalWeight += item.GetComponent<Item>().GetWeight();
        }
    }

    private void FixedUpdate()
    {
        if (room.IsActive())
        {
            bool b = true;
            int counter = 0;
            while (b)
            {
                float rand = Random.Range(0, totalWeight);

                foreach (GameObject item in itemList)
                {
                    rand -= item.GetComponent<Item>().GetWeight();
                    if (rand <= 0)
                    {
                        if (GameObject.Find(item.GetComponent<Item>().GetName()) == null)
                        {
                            if (!item.GetComponent<Item>().IsUnique() || !playerController.HasItem(item.GetComponent<Item>().GetName()))
                            {
                                Instantiate(item, transform.position, Quaternion.identity, room.transform);
                                
                                b = false;
                            }
                        }
                        break;
                    }
                }

                if (b)
                {
                    counter++;
                    if (counter > 10)
                    {
                        Instantiate(itemList[0], transform.position, Quaternion.identity, room.transform);
                        b = false;
                    }
                }
            }

            Destroy(gameObject);
        }
    }
}
