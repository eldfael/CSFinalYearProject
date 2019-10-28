using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    
    SpriteRenderer sprRenderer;
    public Sprite defaultSprite;
    public Sprite rollingSprite;

    public Vector2 direction;
    
    public float movespeed = 5.0f;
    
    public float rollspeed = 9f;
    public float rolltime = 0.5f; // 0.5 per second (for some reason im not sure yet..)
    public float timercount = 0.0f;
    public bool rolling = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (!rolling)
        {
            direction = getDirection().normalized; // get direction and normalize that vector so that each direction moves at equal speeds
        }
        if (direction.magnitude != 0 && Input.GetKeyDown("space")) 
        {
            rolling = true;
            sprRenderer.sprite = rollingSprite;
        }
        

    }

    void FixedUpdate()
    {
        

        if (rolling) 
        {
            timercount += Time.fixedDeltaTime;
            rb.velocity = direction * rollspeed;
            
        }
        else 
        {
            rb.velocity = direction * movespeed;    
        }
        
        if (timercount >= rolltime)
        {
            timercount = 0;
            rolling = false;
            sprRenderer.sprite = defaultSprite; 
        }
    }
    Vector2 getDirection() {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
