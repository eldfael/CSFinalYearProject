using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Component Decleration
    Rigidbody2D playerRigidyBody;
    SpriteRenderer playerSpriteRenderer;
    Camera mainCamera;
    GameController gameController;
    Animator playerAnimator;

    GameObject[] weapons = new GameObject[2];
    public List<string> items = new List<string>();

    Weapon currentWeapon;
    int currentWeaponIndex = 0;


    // Sprite Decleration
    public Sprite defaultSprite;
    public Sprite rollingSprite;

    // Stats Decleration
    int stat_MaxHP;
    int stat_CurrentHP;
    int stat_BonusHP = 0;

    float contactTime = 0.5f;
    float contactTimer = 0.5f;

    int stat_TotalXP = 0;
    int stat_Level = 0;
    int stat_Points = 0;
    
    int stat_VIT = 5;

    int stat_STR = 5;
    int stat_AGI = 5;
    int stat_SKI = 5;
    int stat_HEA = 5;

    int stat_END = 0;
    

    // Movement Decleration
    public float MOVEMENT_SPEED = 6.0f;

    // Rolling Decleration
    public float ROLL_SPEED = 10.0f;
    public float ROLL_DURATION = 0.5f;

    Vector2 rollDirection;
    float rollTimer = 0.0f;
    bool rollKeyDown;
    public bool rollBoolean = false;

    // Attacking Decleration
    public float PROJECTILE_SPEED = 15f;
    public float ATTACK_DURATION = 0.5f;

    bool attackKeyHeld;
    bool attackKeyDown;
    bool interactKeyDown;

    bool attackNext;
    bool interactNext;

    public bool isActive;

    // Inputs Decleration
    Vector2 mousePosition;
    Vector2 keyboardDirection;

    // Key Decleration
    KeyCode rollKey = KeyCode.Mouse1;
    KeyCode attackKey = KeyCode.Mouse0;
    KeyCode interactKey = KeyCode.E;
    KeyCode swapWeaponKey = KeyCode.Space;

    

    void Start()
    {
        playerRigidyBody = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        gameController = GameObject.Find("Game Controller").GetComponent<GameController>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        playerAnimator = GetComponent<Animator>();

        UpdateStats();

        stat_CurrentHP = stat_MaxHP;

        stat_TotalXP = 0;
        stat_Level = 0;
        //stat_Points = 0;

        isActive = true;

        playerSpriteRenderer.sortingLayerName = "Player";
       
    }

    void Awake()
    {

    }

    void Update()
    {
        HandleInput();

        if (isActive)
        {
            HandleWeaponSwap();

            HandleRolling();

            HandleKeyDown();
        }
    }

    void FixedUpdate()
    {

        HandleHP();

        UpdateStats();

        if (isActive)
        {
            HandleCamera();

            HandleLevelUp();

            HandleMovement();

            HandleAttacking();

            HandleTimers();

            HandleInteraction();

            
        }
        
        HandleAnimation();
        
        ResetKeyDown();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

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

    private void HandleAnimation()
    {
        playerAnimator.SetFloat("Speed", playerRigidyBody.velocity.magnitude);
        
        playerAnimator.SetBool("Rolling", rollBoolean);

        if (!rollBoolean)
        {

            if (mousePosition.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (mousePosition.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void HandleCamera()
    {
        // Called in FixedUpdate

        // Move the camera based on the position of the mouse and the player to be at a point inbetween
        mainCamera.transform.position = new Vector3(
            (transform.position.x * 4f + mousePosition.x) / 5f,
            (transform.position.y * 4f + mousePosition.y) / 5f,
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




    }

    private void HandleKeyDown()
    {
        // Called in Update

        // Check to see if KeyDowns are pressed and turn their respective booleans to true so that on the next Physics Update they are activated
        if (attackKeyDown) { attackNext = true; }
        if (interactKeyDown) { interactNext = true; }
    }

    private void ResetKeyDown()
    {
        // Called in FixedUpdate


        attackNext = false;
        interactNext = false;
    }

    private void HandleInteraction()
    {
        // Called in FixedUpdate

        if (interactNext)
        {
            interactNext = false;
            Collider2D collider = Physics2D.OverlapBox(transform.position, new Vector2(1f, 1f), 0, LayerMask.GetMask("Weapon") + LayerMask.GetMask("Interactable"));
            if (collider != null)
            {
                if (collider.CompareTag("Weapon"))
                {
                    EquipWeapon(collider);
                }

                if (collider.CompareTag("Portal"))
                {
                    stat_CurrentHP = stat_MaxHP;
                    gameController.SwapScene();
                }

                if (collider.CompareTag("Item"))
                {
                    HandleItemPickup(collider);
                }
            }
        }
    }

    private void HandleRolling()
    {
        // Called in Update

        // Check to see if the player has an input direction and that the player is not already rolling if the rolling key is pressed (and stamina above 2)
        if (rollKeyDown && keyboardDirection.magnitude != 0 && !rollBoolean)
        {

            // Set the direction of the roll to the current direction of keyboard input
            rollDirection = keyboardDirection.normalized;
            // Change the layer of the player to "PlayerRolling" instead of "Player"
            gameObject.layer = LayerMask.NameToLayer("Player Rolling");
            // Set rolling boolean to true
            rollBoolean = true;
            // Reset the rolling timer
            rollTimer = 0.0f;

            // Temporary until animations are added // Change the sprite of the player to show rolling
        }
    }

    private void HandleAttacking()
    {
        // Called in FixedUpdate
        if (weapons[currentWeaponIndex] != null && !rollBoolean)
        {
            currentWeapon = weapons[currentWeaponIndex].GetComponent<Weapon>();


            if (currentWeapon.IsAutomatic())
            {
                if (attackKeyHeld && currentWeapon.IsReady())
                {
                    currentWeapon.OnAttack();
                }
            }
            else
            {
                if (attackNext && currentWeapon.IsReady())
                {
                    currentWeapon.OnAttack();
                }
            }
        }
    }

    private void HandleTimers()
    {
        // Called in FixedUpdate


        // Rolling Timers
        if (rollBoolean) { rollTimer += Time.fixedDeltaTime; }
        if (rollTimer >= ROLL_DURATION) { rollBoolean = false; gameObject.layer = LayerMask.NameToLayer("Player"); }

        // Contact Timer
        if (contactTimer <= contactTime)
        {
            contactTimer += Time.fixedDeltaTime;
        }
        
        if (rollBoolean)
        {
            Debug.Log("Rolling");
        }
    }

    private void HandleWeaponSwap()
    {
        // Called in Update

        if (Input.GetKeyDown(swapWeaponKey))
        {
            if (currentWeaponIndex == 0 && weapons[1] != null) { currentWeaponIndex = 1; }
            else if (currentWeaponIndex == 1 && weapons[0] != null) { currentWeaponIndex = 0; }
        }
    }

    private void HandleItemPickup(Collider2D collider)
    {
        items.Add(collider.gameObject.GetComponent<Item>().GetName());
        collider.gameObject.GetComponent<Item>().OnPickup();
        Destroy(collider.gameObject);
    }

    public void HandleDamage(int damage)
    {
        stat_CurrentHP -= Mathf.Clamp(damage - stat_END,1,damage);
        HandleHP();
    }

    public void HandleXPGain(int xp)
    {
        stat_TotalXP += xp;
    }
    public void HandleHPGain(int hp)
    {
        stat_CurrentHP = Mathf.Clamp(stat_CurrentHP + hp, 0, stat_MaxHP);
    }

    void HandleLevelUp()
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

    public void EquipWeapon(Collider2D collision)
    {
        if (weapons[0] != null && weapons[1] != null)
        {
            Debug.Log(weapons.Length);

            weapons[currentWeaponIndex].transform.parent = null;
            SceneManager.MoveGameObjectToScene(weapons[currentWeaponIndex], SceneManager.GetActiveScene());
            weapons[currentWeaponIndex].GetComponent<Weapon>().SetSortingLayer("Collectable");
            weapons[currentWeaponIndex].transform.position = gameObject.transform.position;

            weapons[currentWeaponIndex] = collision.gameObject;
            weapons[currentWeaponIndex].GetComponent<Weapon>().SetSortingLayer("Weapon");
            weapons[currentWeaponIndex].transform.parent = gameObject.transform;
        }
        else if (weapons[0] != null || weapons[1] != null)
        {
            if (currentWeaponIndex == 1)
            {
                weapons[0] = collision.gameObject;
                weapons[0].GetComponent<Weapon>().SetSortingLayer("Weapon");
                weapons[0].transform.parent = gameObject.transform;
            }
            else
            {
                weapons[1] = collision.gameObject;
                weapons[1].GetComponent<Weapon>().SetSortingLayer("Weapon");
                weapons[1].transform.parent = gameObject.transform;
            }
        }
        else
        {
            weapons[currentWeaponIndex] = collision.gameObject;
            weapons[currentWeaponIndex].GetComponent<Weapon>().SetSortingLayer("Weapon");
            weapons[currentWeaponIndex].transform.parent = gameObject.transform;
        }
    }


    void HandleDeath()
    {
        SetInactive();
    }

    public void SetInactive()
    {
        rollBoolean = false; 
        gameObject.layer = LayerMask.NameToLayer("Player");
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

        stat_MaxHP = 5 + stat_VIT + stat_BonusHP;
    }

    public void HandleContactDamage(int damage) {
        if (contactTimer >= contactTime)
        {
            HandleDamage(damage);
            contactTimer = 0;
        }
    }

    public bool HasItem(string itemName)
    {
        foreach (string item in items) 
        { 
            if (item == itemName)
            {
                return true;
            }
        }
        return false;
    }

    // Get methods for variables
    public int GetSTR() { return stat_STR; }
    public int GetAGI() { return stat_AGI; }
    public int GetSKI() { return stat_SKI; }
    public int GetHEA() { return stat_HEA; }
    public int GetVIT() { return stat_VIT; }
    public int GetEND() { return stat_END; }
    public int GetLevel() { return stat_Level; }
    public int GetTotalXP() { return stat_TotalXP; }
    public int GetCurrentHP() { return stat_CurrentHP; }
    public int GetMaxHP() { return stat_MaxHP; }
    public int GetBonusHP() { return stat_BonusHP; }
    public GameObject GetWeapon() { return weapons[currentWeaponIndex]; }

    // Set methods for variables
    public void SetSTR(int newSTR) { stat_STR = newSTR; }
    public void SetAGI(int newAGI) { stat_AGI = newAGI; }
    public void SetSKI(int newSKI) { stat_SKI = newSKI; }
    public void SetHEA(int newHEA) { stat_HEA = newHEA; }
    public void SetVIT(int newVIT) { stat_VIT = newVIT; }
    public void SetEND(int newEND) { stat_END = newEND; }
    public void SetBonusHP(int newBonusHP) { stat_BonusHP = newBonusHP; }
    public void SetCurrentHP(int newCurrentHP) { stat_CurrentHP = newCurrentHP; }
    public void SetLevel(int newLevel) { stat_Level = newLevel; }


}
