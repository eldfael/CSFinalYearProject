using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject xpOrb; // Must be added on prefab
    public GameObject[] drops; // Must be added on prefab
    public EnemyBehaviour enemyBehaviour; // Must be added on prefab

    // Enemy stat decleration
    int enemy_MaxHP;
    int enemy_CurrentHP;
    int enemy_END;

    LevelController levelController;

    private void Start()
    {
        // Get the behaviour script attached to this gameobject
        enemyBehaviour = GetComponent<EnemyBehaviour>();

        levelController = GameObject.Find("Level Controller").GetComponent<LevelController>();
        levelController.SetEnemyCount(levelController.GetEnemyCount() + 1);
        
        // Get the enemy's max HP and set it's current HP to that value
        enemy_MaxHP = enemyBehaviour.GetMaxHP();
        enemy_CurrentHP = enemy_MaxHP;
        
        // Get the enemy's Endurance
        enemy_END = enemyBehaviour.GetRES();
        
        // Run the enemy's start method
        enemyBehaviour.OnStart();


    }


    private void FixedUpdate()
    {
        // Check to see if enemy is dead
        if (enemy_CurrentHP <= 0)
        {
            // If enemy's health reduced to 0 or below run the Death handling method
            HandleDeath();
        }
        else
        {
            // Run the enemy's main behaviour method
            enemyBehaviour.OnFixed();
        }

    }
    
    private void OnCollisionStay2D(Collision2D collision)
    {
        enemyBehaviour.OnContact(collision);
    }

    public void HandleDeath() 
    {
        // If the enemy has any on death events they will be run here
        enemyBehaviour.OnDeath();

        if (drops.Length > 0)
        {
            if (Random.Range(0f, 1f) >= 0.9)
            {
                Instantiate(drops[Random.Range(0, drops.Length - 1)], transform.position, Quaternion.identity);
            }
        }

        // Spawn XP orbs on enemy location then destroy the enemy game object
        for (int x = 0; x < enemyBehaviour.GetXPQuantity(); x++) { Instantiate(xpOrb,transform.position,Quaternion.identity); }

        levelController.SetEnemyCount(levelController.GetEnemyCount() - 1);
        if (levelController.GetEnemyCount() <= 0)
        {
            levelController.SpawnPortal(transform.position);
        }

        Destroy(gameObject);
    }

    public void HandleHit(int damage, Vector2 knockback)
    {
        enemy_CurrentHP -= Mathf.Max(damage - enemy_END, 1);
        if (enemy_CurrentHP <= 0)
        {
            // If enemy's health reduced to 0 or below run the Death handling method
            HandleDeath();
        }
        else if (enemyBehaviour.IsMoveable())
        {
            // If enemy is moveable then call the knockback method
            enemyBehaviour.Knockback(knockback);
        }

    }

}
