using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;

    public Vector2 direction;
    public float PROJECTILE_SPEED;
    public float ATTACKING_INTERVAL;
    public float PROJECTILE_SPRITE_OFFSET = 90;

    public float playerDistance;

    public int stat_MaxHP;
    public int stat_CurrentHP;

    bool isAttacking;
    float attackingTimer;
    void Start()
    {
        isAttacking = false;
        stat_MaxHP = 10;
        stat_CurrentHP = stat_MaxHP;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
        if (stat_CurrentHP <= 0)
        {
            // Enemy Dies
            Destroy(gameObject);
        }

        playerDistance = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y).magnitude;

        if (!isAttacking && playerDistance <= 10) 
        {
            direction = new Vector2 (player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y).normalized;


            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = 2;
            tempProjectile.GetComponent<ProjectileController>().velocity = direction * PROJECTILE_SPEED;

            tempProjectile.GetComponent<Transform>().Rotate(0,0, Mathf.Atan2(direction.y,direction.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);

            isAttacking = true;
            attackingTimer = 0.0f;

        }
        else
        {
            attackingTimer += Time.fixedDeltaTime;
            if (attackingTimer >= ATTACKING_INTERVAL) 
            {
                isAttacking = false;
            }
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            if (!collision.GetComponent<ProjectileController>().creator.Equals(gameObject) && collision.GetComponent<ProjectileController>().playerProjectile == true)
            {
                stat_CurrentHP -= collision.GetComponent<ProjectileController>().projectileDamage;
                Debug.Log("Enemy takes damage");
                
                if (stat_CurrentHP < 1) 
                {
                    // Enemy Dies
                    Destroy(gameObject);
                }
            }
        }
    }
}
