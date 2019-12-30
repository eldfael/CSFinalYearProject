using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrbController : MonoBehaviour
{
    public int xpAmount = 1;
    GameObject playerObject;
    Rigidbody2D rigidBody;
    bool collectable;
    Vector2 initialDirection;
    float timer;
    float aliveTime = 6f;
    Vector2 playerDistance;
    
    Sprite sprite;
    SpriteRenderer spriteRenderer;

    BoxCollider2D boxCollider;

    public void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        

        playerObject = GameObject.FindGameObjectWithTag("Player");
        
        collectable = false;
        
        initialDirection = new Vector2(Random.Range(1f,-1f),Random.Range(1f,-1f)).normalized;
        rigidBody.velocity = initialDirection * Random.Range(8f,4f);
    }

    public void FixedUpdate()
    {
        if (collectable) 
        {
            if (timer >= aliveTime) { Destroy(gameObject); }
            else { timer += Time.fixedDeltaTime; }

            playerDistance = new Vector2(playerObject.transform.position.x - transform.position.x, playerObject.transform.position.y -transform.position.y);

            if (playerDistance.magnitude <= 4) {
                rigidBody.velocity = playerDistance.normalized * 15f;
            }
            else 
            {
                rigidBody.velocity = Vector2.zero;
            }

            if (Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("Player") | LayerMask.GetMask("PlayerRolling")))
            {
                playerObject.GetComponent<PlayerController>().HandleXPGain(1);
                Destroy(gameObject);
            }
        }
        else 
        {
            if (rigidBody.velocity.magnitude <= 2f)
            {
                rigidBody.velocity = Vector2.zero;
                collectable = true;
            }
            else 
            {
                rigidBody.velocity = rigidBody.velocity * 0.95f;
            }
        }

    }
}
