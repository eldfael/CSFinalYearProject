using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHealthRoot : MonoBehaviour, Item
{
    PlayerController playerController;
    string itemName = "Health Root";
    float itemWeight = 1f;
    bool unique = false;

    public void OnPickup()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.SetBonusHP(playerController.GetBonusHP() + 3);
        playerController.UpdateStats();
        playerController.HandleHPGain(3);
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
