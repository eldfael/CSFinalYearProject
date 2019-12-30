using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSkeleton : MonoBehaviour, Weapon
{
    float attackTimer;
    float attackInterval;

    float duration;
    float speed;
    float knockback;
    bool melee;
    bool automatic;
    int damage;
    int staminaCost;
    Vector2 hitbox;

    Camera mainCamera;
    Vector2 mousePosition;

    public Sprite projectileSprite;
    public Sprite weaponSprite;

    SpriteRenderer spriteRenderer;
    GameObject playerObject;
    BoxCollider2D boxCollider;


    public void Start()
    {
        playerObject = GameObject.Find("Player");
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = weaponSprite;
        spriteRenderer.sortingLayerName = "Collectable";

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;

        attackTimer = attackInterval;
    }
    public void Update()
    {
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector2(
            Mathf.Clamp(Input.mousePosition.x, 0, Screen.width),
            Mathf.Clamp(Input.mousePosition.y, 0, Screen.height)));
    }
    public void FixedUpdate()
    {
        if (transform.parent != null && transform.parent.gameObject == playerObject)
        {
            boxCollider.enabled = false;
            attackInterval = 10f / (float)playerObject.GetComponent<PlayerController>().GetAGI();

            if (gameObject.Equals(playerObject.GetComponent<PlayerController>().GetWeapon()))
            {
                if (attackTimer <= attackInterval) { attackTimer += Time.fixedDeltaTime; }
                spriteRenderer.enabled = true;
                if (mousePosition.x > playerObject.transform.position.x) { spriteRenderer.flipY = false; }
                else { spriteRenderer.flipY = true; }

                transform.SetPositionAndRotation(
                    new Vector2(
                        mousePosition.x - playerObject.transform.position.x,
                        mousePosition.y - playerObject.transform.position.y).normalized * 0.55f + (Vector2)playerObject.transform.position,
                    Quaternion.Euler(0, 0, Mathf.Atan2(mousePosition.y - playerObject.transform.position.y, mousePosition.x - playerObject.transform.position.x) * Mathf.Rad2Deg));
            }

            else { spriteRenderer.enabled = false; }
        }
        else
        {
            spriteRenderer.enabled = true;
            boxCollider.enabled = true;
        }



    }
    public void OnAttack()
    {
        attackTimer = 0;

        GameObject projectile = new GameObject();
        ProjectileController projectileController = projectile.AddComponent<ProjectileController>();

        projectileController.Create(
            playerObject, // Creator
            projectileSprite, // Sprite of Projectile
            hitbox, // Size of hitbox
            LayerMask.GetMask("Enemy"),
            new Vector2(playerObject.transform.position.x, playerObject.transform.position.y) + (mousePosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y)).normalized, // Position
            (mousePosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y)).normalized * speed, // Direction and Velocity
            playerObject.GetComponent<PlayerController>().GetSTR() * damage, // Damage
            knockback, // Knockback modifier
            duration, // Duration
            melee // Melee Weapon
            );


        projectile.GetComponent<Transform>().Rotate(0, 0, Mathf.Atan2(mousePosition.y - playerObject.transform.position.y, mousePosition.x - playerObject.transform.position.x) * Mathf.Rad2Deg + 0f, Space.Self);
    }
    public bool IsAutomatic()
    {
        return automatic;
    }

    public bool IsMelee()
    {
        return melee;
    }

    public int GetSTACost()
    {
        return staminaCost;
    }

    public bool IsReady()
    {
        return (attackTimer >= attackInterval);
    }

    public void SetSortingLayer(string layer)
    {
        spriteRenderer.sortingLayerName = layer;
    }
}
