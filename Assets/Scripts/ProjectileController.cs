using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector2 velocity;
    public GameObject creator;
    public bool playerProjectile;
    public Rigidbody2D rb;
    public int projectileDamage;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.velocity = velocity;
        //transform.position = new Vector2(transform.position.x, transform.position.y) + velocity;
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Do not destroy on collision with self
        if (!collision.gameObject.Equals(creator))
        {
            if (collision.tag.Equals("Player")) 
            { 
                if (collision.TryGetComponent<PlayerController>(out PlayerController p) == true && p.rollBoolean == false) 
                {
                    Destroy(gameObject);
                }
            }
            if (collision.tag.Equals("Enemy")) 
            {
                if (playerProjectile == true)
                {
                    Destroy(gameObject);
                }
            }
        }
        if (collision.tag.Equals("Wall"))
        {
            Destroy(gameObject);
        }
        
        
    }
    


}
