using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallShooterController : MonoBehaviour
{
    public GameObject projectile;
    public Vector2 DIRECTION;
    public float PROJECTILE_SPEED;
    public float ATTACKING_INTERVAL;
    public float PROJECTILE_SPRITE_OFFSET = 90;

    bool isAttacking;
    float attackingTimer;
    void Start()
    {
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (!isAttacking) 
        {
            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
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
}
