using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public GameObject XPOrb; // Must be added on prefab
    public int XPOrbQuantity; // Must be set on prefab

    public EnemyBehaviour enemyBehaviour; // Must be added on prefab
    
    // Enemy stat decleration
    public int enemy_MaxHP = 10;
    int enemy_CurrentHP;
    public int enemy_END = 0; // Defaults to 0, unless set otherwise

    private void Start()
    {
        enemyBehaviour = GetComponent<EnemyBehaviour>();
        Debug.Log(enemyBehaviour);
        // Set the enemy's current HP to it's max HP
        enemy_CurrentHP = enemy_MaxHP;
        // Run the enemy's start method
        enemyBehaviour.OnStart();
    }

    private void FixedUpdate()
    {
        // Check to see if enemy is dead
        if (enemy_CurrentHP <= 0)
        {
            // If enemy's health reduced to 0 or below run the Death handling method
            HandleDeath(gameObject, XPOrbQuantity);
        }
        else
        {
            // Run the enemy's main behaviour method
            enemyBehaviour.OnFixed();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check to see if the collision is with a projectile
        if (collision.gameObject.tag.Equals("Projectile")) 
        {
            // Check to see if the projectile collision is a player projectile
            if (collision.gameObject.GetComponent<ProjectileController>().playerProjectile.Equals(true))
            {
                // Cause the enemy to take damage from the projectile, and check to see if the damage reduces the enemy's health below 0
                // Reduces the damage of the projectile by the enemy's Endurance stat, however damage can not be less than 1
                enemy_CurrentHP -= Mathf.Max(collision.gameObject.GetComponent<ProjectileController>().projectileDamage - enemy_END , 1);
                if (enemy_CurrentHP <= 0) 
                {
                    // If enemy's health reduced to 0 or below run the Death handling method
                    HandleDeath(gameObject, XPOrbQuantity);
                }
                
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // If enemy is collinding with a player
        if (collision.gameObject.tag.Equals("Player"))
        {
            // If enemy has contact effect activate it now
            enemyBehaviour.OnContact();
        }
    }

    public void HandleDeath(GameObject enemyObject, int XPQuantity) 
    {
        // If the enemy has any on death events they will be run here
        enemyBehaviour.OnDeath();

        // Spawn XP orbs on enemy location then destroy the enemy game object
        for (int x = 0; x < XPQuantity; x++) { Instantiate(XPOrb,transform.position,Quaternion.identity); }
        Destroy(enemyObject);
    }

}
