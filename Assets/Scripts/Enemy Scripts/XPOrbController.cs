using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrbController : MonoBehaviour
{
    public int XPAmount;
    GameObject player;
    Rigidbody2D rb;
    bool collectable;
    Vector2 initialDirection;

    public void Start()
    {
        XPAmount = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        collectable = false;
        
        initialDirection = new Vector2(Random.Range(1f,-1f),Random.Range(1f,-1f)).normalized;
        rb.velocity = initialDirection * Random.Range(8f,4f);
    }

    public void FixedUpdate()
    {
        if (collectable) 
        { 
            Vector2 playerDistance = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
            if (playerDistance.magnitude <= 4) {
                rb.velocity = playerDistance.normalized * -15f;
            }
            if (Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("Player") | LayerMask.GetMask("PlayerRolling")))
            {
                player.GetComponent<PlayerController>().stat_totalXP++;
                Debug.Log(player.GetComponent<PlayerController>().stat_totalXP);
                Destroy(gameObject);
            }
        }
        else 
        {
            if (rb.velocity.magnitude <= 1f)
            {
                rb.velocity = Vector2.zero;
                collectable = true;
            }
            else 
            {
                rb.velocity = rb.velocity - (initialDirection * 0.2f);
            }
        }

    }
}
