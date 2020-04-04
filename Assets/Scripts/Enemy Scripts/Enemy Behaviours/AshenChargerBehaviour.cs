using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AshenChargerBehaviour : MonoBehaviour, EnemyBehaviour
{
    public Sprite projectileSprite;

    public int MAXHP = 15;
    public int RES = 0;
    public int XPQUANTITY = 5;
    public bool MOVEABLE = true;

    public int PATROLRANGE = 3;
    public int DETECTIONRANGE = 7;

    Vector2 patrolPosition;
    Vector2 movementDirection;
    float movementTime;
    float movementTimer;

    bool detected;
    bool updatePatrolPosition;
    bool fired;
    bool firing;
    float firedTimer;

    Rigidbody2D enemyRigidbody;
    GameObject playerObject;

    Vector2 knockbackDirection;
    float knockbackTimer;

    public int GetMaxHP()
    {
        return MAXHP;
    }

    public int GetRES()
    {
        return RES;
    }

    public int GetXPQuantity()
    {
        return XPQUANTITY;
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

    public void OnContact(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerObject.GetComponent<PlayerController>().HandleContactDamage(GameObject.Find("Game Controller").GetComponent<GameController>().difficulty / 2 + 2);
        }
    }

    public void OnDeath()
    {

    }

    public void OnFixed()
    {
        /*
        if (movementTimer >= movementTime) 
        { 
            detected = (playerObject.transform.position - transform.position).magnitude <= DETECTIONRANGE;
        }

        if (!detected && movementTimer >= movementTime)
        {

            movementTimer = 0;

            bool loopBool = true;
            while (loopBool)
            {

                movementTime = Random.Range(0.5f, 1f);
                movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 2f;

                if (((movementTime * movementDirection) + (Vector2)transform.position - patrolPosition).magnitude <= PATROLRANGE)
                {
                    RaycastHit2D[] hits = new RaycastHit2D[1];
                    GetComponent<CircleCollider2D>().Cast(movementDirection.normalized, hits, movementTime * 2f);

                    if (hits[0].collider == null || !hits[0].collider.CompareTag("Wall"))
                    {
                        loopBool = false;
                    }

                }
            }

        }
        else if (detected && movementTimer >= movementTime)
        {
            movementTimer = 0;

            movementTime = Random.Range(1f, 1.5f);
            movementDirection = (playerObject.transform.position - transform.position).normalized * 3f;

            updatePatrolPosition = true;

            Vector2 shootDirection = (playerObject.transform.position - transform.position).normalized;

            for (int x = -1; x < 2; x++)
            {
                GameObject projectile = new GameObject();
                ProjectileController projectileController = projectile.AddComponent<ProjectileController>();

                projectileController.Create(
                    gameObject, // Creator
                    projectileSprite, // Sprite of Projectile
                    new Vector2(0.6f, 0.6f), // Size of hitbox
                    LayerMask.GetMask("Player"),
                    (Vector2)transform.position + new Vector2(Mathf.Cos((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad), Mathf.Sin((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad)).normalized * 0.25f, // Position
                    new Vector2(Mathf.Cos((x * 0.3f) + Vector2.SignedAngle(Vector2.right,shootDirection) * Mathf.Deg2Rad), Mathf.Sin((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad)).normalized * 8f, // Direction and Velocity
                    2, // Damage
                    0f, // Knockback modifier
                    1.5f, // Duration
                    false // Melee Weapon
                    );

                projectile.transform.parent = null;
                SceneManager.MoveGameObjectToScene(projectile, SceneManager.GetActiveScene());
            }

        }


        if (knockbackTimer < 0.1f)
        {
            enemyRigidbody.velocity = knockbackDirection;
            knockbackTimer += Time.fixedDeltaTime;
        }
        else
        {
            enemyRigidbody.velocity = movementDirection;
            movementTimer += Time.fixedDeltaTime;
        }

        if (movementTimer >= movementTime)
        {
            if (updatePatrolPosition)
            {
                updatePatrolPosition = false;
                patrolPosition = transform.position;
            }
        }
        */

        if (movementTimer >= movementTime && !firing)
        {
            
            detected = (playerObject.transform.position - transform.position).magnitude <= DETECTIONRANGE;
        }
        
        if (!detected && movementTimer >= movementTime)
        {
            if (updatePatrolPosition)
            {
                updatePatrolPosition = false;
                patrolPosition = transform.position;
                Debug.Log("Patrol position updated");
            }

            movementTimer = 0;
            Debug.Log("Not Detected");

            bool loopBool = true;
            while (loopBool)
            {

                movementTime = Random.Range(0.5f, 1f);
                movementDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * 2f;

                if (((movementTime * movementDirection) + (Vector2)transform.position - patrolPosition).magnitude <= PATROLRANGE)
                {
                    RaycastHit2D[] hits = new RaycastHit2D[1];
                    GetComponent<CircleCollider2D>().Cast(movementDirection.normalized, hits, movementTime * 2f);

                    if (hits[0].collider == null || !hits[0].collider.CompareTag("Wall"))
                    {
                        loopBool = false;
                    }

                }
            }
        }
        else if (!firing && movementTimer >= movementTime)
        {
            if (Random.Range(0,3) == 1)
            {
                firing = true;
                fired = false;
                firedTimer = 0;
            }
            else
            {
                movementTimer = 0;

                movementTime = Random.Range(1f, 1.5f);
                movementDirection = (playerObject.transform.position - transform.position).normalized * 3f;

                updatePatrolPosition = true;
            }
        }


        if (movementTimer < movementTime && !firing)
        {
            if (knockbackTimer < 0.1f)
            {
                enemyRigidbody.velocity = knockbackDirection;
                knockbackTimer += Time.fixedDeltaTime;
            }
            else
            {
                enemyRigidbody.velocity = movementDirection;
                movementTimer += Time.fixedDeltaTime;
            }
        }
        else
        {
            enemyRigidbody.velocity = Vector2.zero;

            

            if (firing)
            {
                if(!fired)
                {
                    GetComponent<SpriteRenderer>().color = Color.red;
                }
                else
                {
                    GetComponent<SpriteRenderer>().color = Color.white;
                }

                if (firedTimer >= 0.4f && !fired)
                {
                    Vector2 shootDirection = (playerObject.transform.position - transform.position).normalized;

                    for (int x = -1; x < 2; x++)
                    {
                        GameObject projectile = new GameObject();
                        ProjectileController projectileController = projectile.AddComponent<ProjectileController>();

                        projectileController.Create(
                            gameObject, // Creator
                            projectileSprite, // Sprite of Projectile
                            new Vector2(0.6f, 0.6f), // Size of hitbox
                            LayerMask.GetMask("Player"),
                            (Vector2)transform.position + new Vector2(Mathf.Cos((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad), Mathf.Sin((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad)).normalized * 0.25f, // Position
                            new Vector2(Mathf.Cos((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad), Mathf.Sin((x * 0.3f) + Vector2.SignedAngle(Vector2.right, shootDirection) * Mathf.Deg2Rad)).normalized * 8f, // Direction and Velocity
                            GameObject.Find("Game Controller").GetComponent<GameController>().difficulty / 2 + 2, // Damage
                            0f, // Knockback modifier
                            1.5f, // Duration
                            false // Melee Weapon
                            );

                        projectile.transform.parent = null;
                        SceneManager.MoveGameObjectToScene(projectile, SceneManager.GetActiveScene());
                    }
                    fired = true;
                }

                if (firedTimer >= 1f)
                {
                    firing = false;
                }

                firedTimer += Time.fixedDeltaTime;
            }
        }


    }

    public void OnStart()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        enemyRigidbody = GetComponent<Rigidbody2D>();

        detected = false;
        fired = false;
        firing = false;
        movementTime = 0;
        movementTimer = 0;
        patrolPosition = transform.position;
    }
}
