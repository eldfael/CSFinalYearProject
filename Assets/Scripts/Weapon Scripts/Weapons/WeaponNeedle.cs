using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponNeedle : MonoBehaviour, Weapon
{
    float attackTimer;
    float attackInterval = 0.5f;

    float duration = 0.05f;
    float speed = 0.75f;
    float knockback = 5f;
    bool melee = true;
    bool automatic = false;
    int damage = 6;
    int staminaCost = 2;
    Vector2 hitbox = new Vector2(4f, 0.5f);

    Camera mainCamera;
    Vector2 mousePosition;

    public Sprite projectileSprite;
    public Sprite weaponSprite;

    SpriteRenderer spriteRenderer;
    GameObject playerObject;
    BoxCollider2D boxCollider;

    bool equipped;
    bool held;

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
        if (transform.parent != null && transform.parent.gameObject == playerObject) { held = true; }
        else { held = false; }
        if (held)
        {
            boxCollider.enabled = false;


            if (gameObject.Equals(playerObject.GetComponent<PlayerController>().GetWeapon())) { equipped = true; }
            else { equipped = false; }
            if (equipped)
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
            new Vector2(playerObject.transform.position.x, playerObject.transform.position.y) + (mousePosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y)).normalized * (3 * hitbox.x / 4), // Position
            (mousePosition - new Vector2(playerObject.transform.position.x, playerObject.transform.position.y)).normalized * speed, // Direction and Velocity
            damage + (playerObject.GetComponent<PlayerController>().GetSKI() / 2), // Damage
            knockback, // Knockback modifier
            duration, // Duration
            melee // Melee Weapon
            );

        projectile.transform.parent = null;
        SceneManager.MoveGameObjectToScene(projectile, SceneManager.GetActiveScene());
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
