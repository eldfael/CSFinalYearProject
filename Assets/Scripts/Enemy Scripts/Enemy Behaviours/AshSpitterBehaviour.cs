using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AshSpitterBehaviour : MonoBehaviour, EnemyBehaviour
{
    GameObject playerObject;
    Rigidbody2D enemyRigidBody;

    public int MAXHP = 20;
    public int RES = 0;
    public int XPQUANTITY = 5;
    public bool MOVEABLE = true;

    Vector2 knockbackDirection;
    float knockbackTimer;

    float movementTimer = 0f;
    float movementTime;
    Vector2 movementDirection;

    float idleTimer = 0f;
    float idleTime;
    bool idle = true;
    bool fired = true;

    public Sprite projectileSprite;

    public void OnStart()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyRigidBody = GetComponent<Rigidbody2D>();

        if (Random.Range(0,2) == 1)
        {
            idleTime = Random.Range(1f, 2f);
        }
        else
        {
            idleTime = 0;
        }

        knockbackTimer = 0.1f;
    }

    public void OnFixed()
    {
        if (idle)
        {
            if (idleTimer >= idleTime)
            {
                idle = false;
                idleTimer = 0f;

                movementTime = Random.Range(2f, 3f);
                movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 1.5f;
            }
            else
            {
                idleTimer += Time.fixedDeltaTime;
            }
        }
        else
        {
            if (movementTimer >= movementTime)
            {
                idle = true;
                if (Random.Range(0,2) == 1) { fired = false; }
                movementTimer = 0f;
                
                idleTime = Random.Range(1f, 2f);
            }
            else
            {
                movementTimer += Time.fixedDeltaTime;
            }
        }
        

        if (idleTimer >= 0.5f && !fired)
        {
            
            for (int y = -1; y < 2; y++) {
                for (int x = -1; x < 2; x++)
                {
                    if (y != 0 || x != 0) {
                        Debug.Log(x);
                        Debug.Log(y);
                        Debug.Log("SHOOT");
                        GameObject projectile = new GameObject();
                        ProjectileController projectileController = projectile.AddComponent<ProjectileController>();

                        projectileController.Create(
                            gameObject, // Creator
                            projectileSprite, // Sprite of Projectile
                            new Vector2(0.7f, 0.7f), // Size of hitbox
                            LayerMask.GetMask("Player"),
                            transform.position, // Position
                            new Vector2(x,y).normalized * 7f, // Direction and Velocity
                            2, // Damage
                            0f, // Knockback modifier
                            1f, // Duration
                            false // Melee Weapon
                            );

                        projectile.transform.parent = null;
                        SceneManager.MoveGameObjectToScene(projectile, SceneManager.GetActiveScene());
                    }
                } 
            }
            fired = true;
        }
        


        
        if (knockbackTimer < 0.1f)
        {
            enemyRigidBody.velocity = knockbackDirection;
            knockbackTimer += Time.fixedDeltaTime;
        }
        else if (idle)
        {
            enemyRigidBody.velocity = Vector2.zero;
        }
        else
        {
            enemyRigidBody.velocity = movementDirection;
        }

        if (idle && !fired)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }


    }

    public void OnDeath()
    {

    }

    public void OnContact()
    {

    }

    public bool IsMoveable()
    {
        return MOVEABLE;
    }

    public void Knockback(Vector2 direction)
    {
        knockbackDirection = direction;
        knockbackTimer = 0f;
    }

    public int GetXPQuantity()
    {
        return XPQUANTITY;
    }

    public int GetMaxHP()
    {
        return MAXHP;
    }

    public int GetRES()
    {
        return RES;
    }
}
