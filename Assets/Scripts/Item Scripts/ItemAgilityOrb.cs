using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAgilityOrb : MonoBehaviour, Item
{
    PlayerController playerController;
    string itemName = "Agility Orb";
    float itemWeight = 1f;
    bool unique = false;

    public void OnPickup()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetAGI(playerController.GetAGI() + 1);
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
