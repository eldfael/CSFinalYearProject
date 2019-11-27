using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public Vector2 velocity;
    public GameObject creator;
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
        if (collision.gameObject.Equals(creator) || collision.tag.Equals("Projectile"))
        {
            // Do not destroy on collision with self or objects with tag projectile
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

}
