using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHeatOrb : MonoBehaviour, Item
{
    PlayerController playerController;
    string itemName = "Heat Orb";
    float itemWeight = 0.75f;
    bool unique = false;

    public void OnPickup()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetHEA(playerController.GetHEA() + 1);
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
