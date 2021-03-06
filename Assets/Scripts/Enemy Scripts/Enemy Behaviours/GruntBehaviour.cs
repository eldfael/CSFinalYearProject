﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntBehaviour : MonoBehaviour, EnemyBehaviour
{
    GameObject playerObject;
    Rigidbody2D enemyRigidBody;
    
    bool idle;
    float idleTimer;

    Vector2 knockbackDirection;
    float knockbackTimer;

    float contactTimer;

    public float GRUNT_MOVESPEED = 3f;
    public float DETECTION_RADIUS = 6f;
    float IDLETIMER_THRESHHOLD = 0.5f;
    float CONTACTTIMER_THRESHHOLD = 0.5f;
    float KNOCKBACK_THRESHHOLD = 0.1f;

    public int CONTACTDAMAGE = 2;
    public int MAXHP = 20;

    public void OnStart()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyRigidBody = GetComponent<Rigidbody2D>();

        idle = true;      
        contactTimer = CONTACTTIMER_THRESHHOLD;
    }

    public void OnFixed()
    {
        if (playerObject == null)
        {
            playerObject = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            // Create a vector that is the direction and magnitutde of the player from the enemy
            Vector2 playerVector = new Vector2(
                playerObject.transform.position.x - transform.position.x,
                playerObject.transform.position.y - transform.position.y);

            // Check if player is within a set radius
            if (playerVector.magnitude <= DETECTION_RADIUS)
            {
                idle = false;
                idleTimer = 0;
            }

            // If enemy is NOT idle add to the idle timer
            if (!idle)
            {
                idleTimer += Time.fixedDeltaTime;

                // If idle timer reaches certain threshold set idle to true and movementspeed to 0
                if (idleTimer >= IDLETIMER_THRESHHOLD) { idle = true; enemyRigidBody.velocity = Vector2.zero; }
            }

            // If enemy isn't idle move towards the player
            if (!idle) { enemyRigidBody.velocity = playerVector.normalized * GRUNT_MOVESPEED; }

            if (idle)
            {
                enemyRigidBody.velocity = Vector2.zero;
            }


            
            if (knockbackTimer <= KNOCKBACK_THRESHHOLD)
            {
                enemyRigidBody.velocity = knockbackDirection;
                knockbackTimer += Time.fixedDeltaTime;
            }



        }


    }

    public void OnDeath()
    { 
    
    }

    public void OnContact(Collision2D collision)
    {
        // If contact timer is at the threshhold damage player for contact
        if (collision.gameObject.Equals(playerObject))
        {
            playerObject.GetComponent<PlayerController>().HandleContactDamage(CONTACTDAMAGE);
        }
         
    }

    public bool IsMoveable() { return true; }

    public void Knockback(Vector2 direction) 
    { 
        knockbackDirection = direction;
        knockbackTimer = 0f; 
    }

    public int GetXPQuantity()
    {
        return 3;
    }
    public int GetMaxHP()
    {
        return MAXHP;
    }
    public int GetRES()
    {
        return 0;
    }
}
