using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnduranceRoot : MonoBehaviour, Item
{
    PlayerController playerController;
    string itemName = "Endurance Root";
    float itemWeight = 0.5f;
    bool unique = true;
    
    public void OnPickup()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetEND(playerController.GetEND() + 1);
    }

    public string GetName()
    {
        return itemName;
    }
    public float GetWeight()
    {
        return itemWeight;
    }
    public bool IsUnique()
    {
        return unique;
    }
}
