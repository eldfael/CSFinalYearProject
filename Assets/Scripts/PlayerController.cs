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
    public int stat_CurrentHP;

    public int stat_MaxSTA;
    public int stat_CurrentSTA;
    float stat_STATimer = 0f;
    float stat_STARegenTime = 1f;

    int stat_totalXP;

    int stat_END = 0;

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
    bool attackKeyDown;
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

        stat_MaxSTA = 10;
        stat_CurrentSTA = stat_MaxSTA;

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
        handleHP();

        handleSTA();

        handleCamera();

        handleMovement();

        handleAttacking();

        handleTimers();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Projectile"))
        {
            handleDamage(collision.gameObject.GetComponent<ProjectileController>().projectileDamage);
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
        // Check to see if the key to attack has been pushed
        attackKeyDown = Input.GetKeyDown(attackKey);


    }

    private void handleRolling() 
    { 
        // Called in Update

        // Check to see if the player has an input direction and that the player is not already rolling if the rolling key is pressed (and stamina above 1)
        if (rollKeyDown && keyboardDirection.magnitude != 0  && !rollBoolean && stat_CurrentSTA >= 1) 
        {
            // Remove 1 Stamina
            handleSTAChange(-1);
            // Set the direction of the roll to the current direction of keyboard input
            rollDirection = keyboardDirection.normalized;
            // Change the layer of the player to "PlayerRolling" instead of "Player"
            gameObject.layer = LayerMask.NameToLayer("Player Rolling");
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
        // Called in FixedUpdate

        // To be worked on 
        // Weapons need to be implemented through the use of a seperate script called from the handleAttacking() method

        if (!attackBoolean && !rollBoolean && attackKeyHeld && stat_CurrentSTA >= 1) {

            handleSTAChange(-1);

            GameObject tempProjectile = Instantiate(projectile,transform.position,Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().playerProjectile = true;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = 2;
            tempProjectile.GetComponent<ProjectileController>().velocity = (mousePosition - new Vector2(transform.position.x,transform.position.y)).normalized * PROJECTILE_SPEED;
            
            tempProjectile.GetComponent<Transform>().Rotate(0,0, Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);
            tempProjectile.layer = LayerMask.NameToLayer("Player Projectile");

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

        // Stamina regeneration
        if (stat_CurrentSTA < stat_MaxSTA) { stat_STATimer += Time.fixedDeltaTime; }
        
    }

    public void handleDamage(int damage) 
    {
        stat_CurrentHP -= damage - stat_END;
        Debug.Log("Player takes " + damage + " Damage");
        handleHP();
    }

    public void handleXPGain()
    {
        stat_totalXP++;
        Debug.Log(stat_totalXP);
    }

    void handleHP()
    {
        // Called in FixedUpdate

        // Make sure HP stays in the range 0 - MaxHP
        Mathf.Clamp(stat_CurrentHP,0,stat_MaxHP);
        // Check to see if HP is at 0 and handle player death if it is at 0
        if (stat_CurrentHP == 0) { handleDeath(); }
    }

    void handleSTA()
    {
        // Called in FixedUpdate

        // Make sure Stamina is in range of 0 - MaxSTA
        Mathf.Clamp(stat_CurrentSTA, 0, stat_MaxSTA);
        // Regen stamina
        if (stat_STATimer >= stat_STARegenTime) 
        {
            stat_CurrentSTA++;
            stat_STATimer = 0f;
        }
        Debug.Log(stat_CurrentSTA);


    }

    void handleSTAChange(int change)
    {
        stat_CurrentSTA += change;
    }

    void handleDeath() 
    {
        Destroy(gameObject);
    }

    public float getHPPercentage()
    {
        return ((float)stat_CurrentHP / (float)stat_MaxHP);
    }

    public float getSTAPercentage()
    {
        return ((float)stat_CurrentSTA / (float)stat_MaxSTA);
    }
    

}
