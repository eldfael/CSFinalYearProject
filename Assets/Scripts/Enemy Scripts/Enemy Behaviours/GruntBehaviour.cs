using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GruntBehaviour : MonoBehaviour, EnemyBehaviour
{
    GameObject playerObject;
    Collider2D enemyCollider;
    Rigidbody2D enemyRigidBody;
    
    bool idle;
    float idleTimer;

    float contactTimer;

    public float GRUNT_MOVESPEED = 3f;
    public float DETECTION_RADIUS = 10f;
    float IDLETIMER_THRESHHOLD = 0.5f;
    float CONTACTTIMER_THRESHHOLD = 0.5f;

    public int CONTACTDAMAGE = 2;

    public void OnStart()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyCollider = GetComponent<BoxCollider2D>();
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

            if (contactTimer <= CONTACTTIMER_THRESHHOLD)
            {
                contactTimer += Time.fixedDeltaTime;
            }
        }


    }

    public void OnDeath()
    { 
    
    }

    public void OnContact()
    {
        // If contact timer is at the threshhold damage player for contact
        if (contactTimer >= CONTACTTIMER_THRESHHOLD) 
        {
            contactTimer = 0;
            playerObject.GetComponent<PlayerController>().handleDamage(CONTACTDAMAGE);
        }
    }
}
