using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemyBehaviour
{
    void OnStart();
    void OnFixed();
    void OnDeath();
    void OnContact(Collision2D collision);
    bool IsMoveable();
    void Knockback(Vector2 direction);
    int GetXPQuantity();
    int GetMaxHP();
    int GetRES();

}
