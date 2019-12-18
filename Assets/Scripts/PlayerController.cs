using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Component Decleration
    Rigidbody2D playerRigidyBody;
    SpriteRenderer playerSpriteRenderer;
    Camera mainCamera;
    GameController gameController;


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

    public int stat_TotalXP;
    public int stat_Level;
    int stat_Points = 0;

    int stat_END = 0;

    int stat_STR = 5;
    int stat_AGI = 5;
    int stat_VIT = 5;

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

    bool interactKeyDown;

    public bool isActive;

    public float PROJECTILE_SPRITE_OFFSET = 90f;

    // Inputs Decleration
    Vector2 mousePosition;
    Vector2 keyboardDirection;

    // Key Decleration
    KeyCode rollKey = KeyCode.Mouse1;
    KeyCode attackKey = KeyCode.Mouse0;
    KeyCode interactKey = KeyCode.E;


    void Start()
    {
        playerRigidyBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        stat_MaxHP = 10;
        stat_CurrentHP = stat_MaxHP;

        stat_MaxSTA = 10;
        stat_CurrentSTA = stat_MaxSTA;

        stat_TotalXP = 0;
        stat_Level = 0;
        //stat_Points = 0;

        isActive = true;
    }

    void Awake()
    {
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
    }

    void Update()
    {
        HandleInput();

        if (isActive)
        {
            HandleRolling();
        }
    }

    void FixedUpdate()
    {

        HandleHP();

        HandleSTA();

        UpdateStats();

        if (isActive)
        {
            HandleCamera();

            HandleLevel();

            HandleMovement();

            HandleAttacking();

            HandleTimers();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Projectile"))
        {
            HandleDamage(collision.gameObject.GetComponent<ProjectileController>().projectileDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal")) 
        {
            gameController.SwapScene();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {

    }


    private void HandleMovement()
    {
        // Called in FixedUpdate

        // If the player is not rolling move normally according to keyboard inputs
        if (!rollBoolean) playerRigidyBody.velocity = keyboardDirection.normalized * MOVEMENT_SPEED;
        // Else the player must be rolling and move accordingly
        else playerRigidyBody.velocity = rollDirection * ROLL_SPEED;
    }

    private void HandleCamera()
    {
        // Called in FixedUpdate

        // Move the camera based on the position of the mouse and the player to be at a point inbetween
        mainCamera.transform.position = new Vector3(
            (transform.position.x * 2.5f + mousePosition.x) / 3.5f,
            (transform.position.y * 2.5f + mousePosition.y) / 3.5f,
            -100);
    }

    private void HandleInput()
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

        // Check to see if the key to interact has been pushed
        interactKeyDown = Input.GetKeyDown(interactKey);
        if (interactKeyDown) { Debug.Log("KEYDOWN"); }


    }

    private void HandleRolling()
    {
        // Called in Update

        // Check to see if the player has an input direction and that the player is not already rolling if the rolling key is pressed (and stamina above 2)
        if (rollKeyDown && keyboardDirection.magnitude != 0 && !rollBoolean && stat_CurrentSTA >= 2)
        {
            // Remove 1 Stamina
            HandleSTAChange(-2);
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

    private void HandleAttacking()
    {
        // Called in FixedUpdate

        // To be worked on 
        // Weapons need to be implemented through the use of a seperate script called from the handleAttacking() method

        if (!attackBoolean && !rollBoolean && attackKeyHeld && stat_CurrentSTA >= 1) {

            HandleSTAChange(-1);

            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);

            tempProjectile.GetComponent<ProjectileController>().creator = gameObject;
            tempProjectile.GetComponent<ProjectileController>().playerProjectile = true;
            tempProjectile.GetComponent<ProjectileController>().projectileDamage = stat_STR/2;
            tempProjectile.GetComponent<ProjectileController>().velocity = (mousePosition - new Vector2(transform.position.x, transform.position.y)).normalized * PROJECTILE_SPEED;

            tempProjectile.GetComponent<Transform>().Rotate(0, 0, Mathf.Atan2(mousePosition.y - transform.position.y, mousePosition.x - transform.position.x) * Mathf.Rad2Deg + PROJECTILE_SPRITE_OFFSET, Space.Self);
            tempProjectile.layer = LayerMask.NameToLayer("Player Projectile");

            attackBoolean = true;
            attackTimer = 0.0f;
        }

    }

    private void HandleTimers()
    {
        // Called in FixedUpdate

        // Attacking Timers
        // If the player is attacking
        if (attackBoolean) { attackTimer += Time.fixedDeltaTime; }
        if (attackTimer >= ATTACK_DURATION / ((float)stat_AGI / 5)) { attackBoolean = false; }

        // Rolling Timers
        if (rollBoolean) { rollTimer += Time.fixedDeltaTime; }
        if (rollTimer >= ROLL_DURATION) { rollBoolean = false; playerSpriteRenderer.sprite = defaultSprite; gameObject.layer = LayerMask.NameToLayer("Player"); }

        // Stamina regeneration
        if (stat_CurrentSTA < stat_MaxSTA) { stat_STATimer += Time.fixedDeltaTime; }

    }

    public void HandleDamage(int damage)
    {
        stat_CurrentHP -= damage - stat_END;
        HandleHP();
    }

    public void HandleXPGain(int xp)
    {
        stat_TotalXP += xp;
    }

    void HandleLevel()
    {
        if (stat_TotalXP >= GetLevelThreshhold(stat_Level + 1))
        {
            stat_Level ++;
            stat_Points  ++;
        }
    }

    public int GetLevelThreshhold(int level)
    {
        return (4 * (level * level) + 5 * level);
    }

    void HandleHP()
    {
        // Called in FixedUpdate

        // Make sure HP stays in the range 0 - MaxHP
        stat_CurrentHP = Mathf.Clamp(stat_CurrentHP, 0, stat_MaxHP);
        // Check to see if HP is at 0 and handle player death if it is at 0
        if (stat_CurrentHP == 0) { HandleDeath(); }
    }

    void HandleSTA()
    {
        // Called in FixedUpdate

        // Regen stamina
        if (stat_STATimer >= stat_STARegenTime)
        {
            HandleSTAChange(1);
            stat_STATimer = 0f;
        }


    }

    public void HandleSTAChange(int change)
    {
        stat_CurrentSTA = Mathf.Clamp(stat_CurrentSTA += change, 0, stat_MaxSTA);
    }

    void HandleDeath()
    {
        isActive = false;
    }

    public float getHPPercentage()
    {
        return Mathf.Clamp01(((float)stat_CurrentHP / (float)stat_MaxHP));
    }

    public float getSTAPercentage()
    {
        return Mathf.Clamp01(((float)stat_CurrentSTA / (float)stat_MaxSTA));
    }

    public void SetInactive()
    {
        rollBoolean = false; playerSpriteRenderer.sprite = defaultSprite; gameObject.layer = LayerMask.NameToLayer("Player");
        
        playerRigidyBody.velocity = Vector2.zero;
        isActive = false;
    }

    public void SetActive()
    {
        isActive = true;
    }

    // Stat point methods
    public int GetStatPoints()
    {
        // Return unspent stat points
        return stat_Points;
    }

    public void SetStatPoints(int newStatPoints)
    {
        stat_Points = newStatPoints;
    }

    public void UpdateStats()
    {
        // Called in FixedUpdate

        // Updates variables based on current player stats

        stat_MaxSTA = stat_STR * 2;
        stat_MaxHP = stat_VIT * 2;
        
    }

    // Get methods for variables
    public int GetSTR() { return stat_STR;}
    public int GetAGI() { return stat_AGI; }
    public int GetVIT() { return stat_VIT; }
    public int GetLevel() { return stat_Level; }


    // Set methods for stats
    public void SetSTR(int newSTR) { stat_STR = newSTR; }
    public void SetAGI(int newAGI) { stat_AGI = newAGI; }
    public void SetVIT(int newVIT) { stat_VIT = newVIT; }
    public void SetLevel(int newLevel) { stat_Level = newLevel; }


}
