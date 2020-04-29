using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStrengthOrb : MonoBehaviour, Item
{
    PlayerController playerController;
    string itemName = "Strength Orb";
    float itemWeight = 1f;
    bool unique = false;

    public void OnPickup()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetSTR(playerController.GetSTR() + 1);
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
