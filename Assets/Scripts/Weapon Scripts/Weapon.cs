using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Weapon
{
    void OnAttack();
    bool IsAutomatic();
    bool IsMelee();
    int GetSTACost();
    bool IsReady();
    void SetSortingLayer(string s);
}
