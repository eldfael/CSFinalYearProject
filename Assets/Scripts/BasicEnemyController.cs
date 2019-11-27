using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    public GameObject projectile;
    public Vector2 DIRECTION;
    public float PROJECTILE_SPEED;
    public float ATTACKING_INTERVAL;
    public float PROJECTILE_SPRITE_OFFSET = 90;

    public int stat_MaxHP;
    public int stat_CurrentHP;

    bool isAttacking;
    float attackingTimer;
    void Start()
    {
        isAttacking = false;
        stat_MaxHP = 10;
        stat_CurrentHP = stat_MaxHP;
    }

    void FixedUpdate()
    {
        if (stat_CurrentHP < 1)
        {
            // Enemy Dies
            Destroy(gameObject);
        }

        if (!isAttacking) 
        {
            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = 2;
            tempProjectile.GetComponent<ProjectileController>().velocity = DIRECTION * PROJECTILE_SPEED;
            tempProjectile.GetComponent<Transform>().Rotate(0,0, Mathf.Atan2(DIRECTION.y,DIRECTION.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);

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
