using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Component Decleration
    Rigidbody2D playerRigidyBody;
    SpriteRenderer playerSpriteRenderer;
    Camera mainCamera;
    
    public GameObject projectile;

    // Sprite Decleration
    public Sprite defaultSprite;
    public Sprite rollingSprite;

    // Stats Decleration
    public int stat_MaxHP;
    int stat_CurrentHP;
    public int stat_MaxSTA;
    int stat_CurrentSTA;
    int stat_totalXP;

    // Movement Decleration
    public float MOVEMENT_SPEED = 6.0f;

    // Rolling Decleration
    public float ROLL_SPEED = 10.0f;
    public float ROLL_DURATION = 0.5f; // 0.5 per second (for some reason im not sure yet..)

    Vector2 rollDirection;
    float rollTimer = 0.0f;
    bool rollKeyDown;
    public bool rollBoolean = false;

    // Attacking Decleration
    public float PROJECTILE_SPEED = 15f;
    public float ATTACK_DURATION = 0.5f;

    float attackTimer = 0.0f;
    bool attackKeyHeld;
    bool attackBoolean = false;

    public float PROJECTILE_SPRITE_OFFSET = 90f;

    // Inputs Decleration
    Vector2 mousePosition;
    Vector2 keyboardDirection;

    // Key Decleration
    KeyCode rollKey = KeyCode.Mouse1;
    KeyCode attackKey = KeyCode.Mouse0;


    void Start()
    {
        playerRigidyBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        
        stat_MaxHP = 10;
        stat_CurrentHP = stat_MaxHP;

        stat_totalXP = 0;
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
        if (stat_CurrentHP <= 0)
        {
            Destroy(gameObject);
        }

        handleCamera();

        handleMovement();

        handleAttacking();

        handleTimers();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!rollBoolean) { 
            if (collision.gameObject.tag == "Projectile") {
                if (!collision.GetComponent<ProjectileController>().creator.Equals(gameObject))
                {
                    stat_CurrentHP -= collision.GetComponent<ProjectileController>().projectileDamage;
                    Debug.Log("Player takes damage");
                    if (stat_CurrentHP <= 0)
                    {
                        // Player Dies
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }


    private void handleMovement() 
    { 
        // Called in FixedUpdate

        // If the player is not rolling move normally according to keyboard inputs
        if (!rollBoolean) playerRigidyBody.velocity = keyboardDirection.normalized * MOVEMENT_SPEED;
        // Else the player must be rolling and move accordingly
        else playerRigidyBody.velocity = rollDirection * ROLL_SPEED; 
    }

    private void handleCamera()
    {
        // Called in FixedUpdate

        // Move the camera based on the position of the mouse and the player to be at a point inbetween
        mainCamera.transform.position = new Vector3(
            (transform.position.x * 2.5f + mousePosition.x)/3.5f,
            (transform.position.y * 2.5f + mousePosition.y)/3.5f,
            -100);
    }

    private void handleInput()
    {
        // Called in Update

        // Get the input direction of the keyboard in the form of a Vector 2
        keyboardDirection = new Vector2(
            Input.GetAxisRaw("Horizontal"), 
            Input.GetAxisRaw("Vertical"));
        
        // Get the position of the mouse on the camera and then convert it to co-ordinates in the scene
        mousePosition = mainCamera.ScreenToWorldPoint(new Vector2(
            Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), 
            Mathf.Clamp(Input.mousePosition.y, 0, Screen.height)));
        
        // Check to see if the key to roll has been pushed
        rollKeyDown = Input.GetKeyDown(rollKey);

        // Check to see if the key to attack is being held
        attackKeyHeld = Input.GetKey(attackKey);


    }

    private void handleRolling() 
    { 
        // Called in Update

        // Check to see if the player has an input direction and that the player is not already rolling if the rolling key is pressed
        if (rollKeyDown && keyboardDirection.magnitude != 0  && !rollBoolean) 
        {
            // Set the direction of the roll to the current direction of keyboard input
            rollDirection = keyboardDirection.normalized;
            // Change the layer of the player to "PlayerRolling" instead of "Player"
            gameObject.layer = LayerMask.NameToLayer("PlayerRolling");
            // Set rolling boolean to true
            rollBoolean = true;
            // Reset the rolling timer
            rollTimer = 0.0f;

            // Temporary until animations are added // Change the sprite of the player to show rolling
            playerSpriteRenderer.sprite = rollingSprite;
        }
    }

    private void handleAttacking() 
    {
        if (!attackBoolean && !rollBoolean && attackKeyHeld) {

            GameObject tempProjectile = Instantiate(projectile,transform.position,Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().playerProjectile = true;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = 2;
            tempProjectile.GetComponent<ProjectileController>().velocity = (mousePosition - new Vector2(transform.position.x,transform.position.y)).normalized * PROJECTILE_SPEED;
            
            tempProjectile.GetComponent<Transform>().Rotate(0,0, Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);

            attackBoolean = true;
            attackTimer = 0.0f;
        }
        
    }

    private void handleTimers() 
    {
        // Called in FixedUpdate

        // Attacking Timers
        // If the player is attacking
        if (attackBoolean) { attackTimer += Time.fixedDeltaTime; }
        if (attackTimer >= ATTACK_DURATION) { attackBoolean = false; }

        // Rolling Timers
        if (rollBoolean) { rollTimer += Time.fixedDeltaTime; }
        if (rollTimer >= ROLL_DURATION) { rollBoolean = false; playerSpriteRenderer.sprite = defaultSprite; gameObject.layer = LayerMask.NameToLayer("Player"); }
        
    }

    public void handleDamage(int damage) 
    {
        stat_CurrentHP -= damage;
        Debug.Log("Player takes " + damage + " Damage");
        if (stat_CurrentHP <= 0) 
        {
            //Die
            Destroy(gameObject);
        }
    }

    public void handleXPGain()
    {
        stat_totalXP++;
        Debug.Log(stat_totalXP);
    }
    

}
