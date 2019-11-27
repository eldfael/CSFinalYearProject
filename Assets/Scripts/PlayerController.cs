﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Component Decleration
    Rigidbody2D rb;
    SpriteRenderer sprRenderer;
    Camera mainCamera;
    
    public GameObject projectile;

    // Sprite Decleration
    public Sprite defaultSprite;
    public Sprite rollingSprite;

    // Stats Decleration
    public int stat_MaxHP;
    public int stat_CurrentHP;
    public int stat_MaxSTA;
    public int stat_CurrentSTA;

    // Movement Decleration
    public float MOVEMENT_SPEED = 5.0f;

    // Rolling Decleration
    public float ROLLING_SPEED = 9.0f;
    public float ROLLING_DURATION = 0.5f; // 0.5 per second (for some reason im not sure yet..)

    public Vector2 rollingDirection;
    public float rollingTimer = 0.0f;
    public bool rollingKeyDown;
    public bool isRolling = false;

    // Attacking Decleration
    public float PROJECTILE_SPEED = 15f;
    public float PROJECTILE_INTERVAL = 0.5f;

    public float attackingTimer = 0.0f;
    public bool attackingKeyHeld;
    public bool isAttacking = false;

    public float PROJECTILE_SPRITE_OFFSET = 90f;

    // Inputs Decleration
    public Vector2 mousePosition;
    public Vector2 keyboardDirection;

    // Key Decleration
    public KeyCode rollingKey = KeyCode.Mouse1;
    public KeyCode attackingKey = KeyCode.Mouse0;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
        
        stat_MaxHP = 10;
        stat_CurrentHP = stat_MaxHP;
    }

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }
    void Update()
    {
        handleInput();

        handleRolling();
    }
    void FixedUpdate()
    {
        if (stat_CurrentHP < 1)
        {
            Destroy(gameObject);
        }

        handleCamera();

        handleMovement();

        handleAttacking();

        handleTimers();
    }
    private void handleMovement() 
    { 
        // If the player is not rolling move normally according to keyboard inputs
        if (!isRolling) rb.velocity = keyboardDirection.normalized * MOVEMENT_SPEED;

        // Else the player must be rolling and move accordingly
        else rb.velocity = rollingDirection * ROLLING_SPEED; 
    }

    private void handleCamera()
    {
        mainCamera.transform.position = new Vector3(
            (transform.position.x * 2.5f + mousePosition.x)/3.5f,
            (transform.position.y * 2.5f + mousePosition.y)/3.5f,
            -100);
    }

    private void handleInput() 
    {
        keyboardDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
        
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector2(
            Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), 
            Mathf.Clamp(Input.mousePosition.y, 0, Screen.height)));
        
        rollingKeyDown = Input.GetKeyDown(rollingKey);
        attackingKeyHeld = Input.GetKey(attackingKey);


    }

    private void handleRolling() 
    { 
        if (keyboardDirection.magnitude != 0 && rollingKeyDown && !isRolling) 
        {
            rollingDirection = keyboardDirection.normalized;
            
            isRolling = true;
            rollingTimer = 0.0f;

            sprRenderer.sprite = rollingSprite;
        }
    }

    private void handleAttacking() 
    {
        if (!isAttacking && !isRolling && attackingKeyHeld) {

            GameObject tempProjectile = Instantiate(projectile,transform.position,Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = 2;
            tempProjectile.GetComponent<ProjectileController>().velocity = (mousePosition - new Vector2(transform.position.x,transform.position.y)).normalized * PROJECTILE_SPEED;
            
            tempProjectile.GetComponent<Transform>().Rotate(0,0, Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);

            isAttacking = true;
            attackingTimer = 0.0f;
        }
        
    }

    private void handleTimers() 
    {
        // Attacking Timers
        if (isAttacking) { attackingTimer += Time.fixedDeltaTime; }
        if (attackingTimer >= PROJECTILE_INTERVAL) { isAttacking = false; }

        // Rolling Timers
        if (isRolling) { rollingTimer += Time.fixedDeltaTime; }
        if (rollingTimer >= ROLLING_DURATION) { isRolling = false; sprRenderer.sprite = defaultSprite; }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile") {
            if (!collision.GetComponent<ProjectileController>().creator.Equals(gameObject))
            {
                stat_CurrentHP -= collision.GetComponent<ProjectileController>().projectileDamage;
                Debug.Log("Player takes damage");
                if (stat_CurrentHP < 1)
                {
                    // Player Dies
                    Destroy(gameObject);
                }
            }
        }   
    }
    
}