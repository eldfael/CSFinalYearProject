using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item
{
    void OnPickup();
    string GetName();
    float GetWeight();
    bool IsUnique();
}
