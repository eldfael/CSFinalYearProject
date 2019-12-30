using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Vector2 velocity;
    GameObject creator;
    Rigidbody2D projRigidbody;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    int damage;
    float duration;
    float timer;
    float knockback;

    bool isMelee;
    List<GameObject> hits = new List<GameObject>();
    ContactFilter2D filter;
    List<Collider2D> collisions = new List<Collider2D>();

    void FixedUpdate()
    {
        projRigidbody.velocity = velocity;
        if ( timer >=  duration) { Destroy(gameObject); }
        else { timer += Time.fixedDeltaTime; }

        Physics2D.OverlapCollider(boxCollider,filter,collisions);
        
        if (collisions.Count > 0)
        {

            collisions.ForEach(delegate (Collider2D c)
            {
                if (c.CompareTag("Player") && !hits.Contains(c.gameObject))
                {
                    c.gameObject.GetComponent<PlayerController>().HandleDamage(GetDamage());
                    if (!isMelee) { Destroy(gameObject); }
                    else { hits.Add(c.gameObject); }
                }
                if (c.CompareTag("Enemy") && !hits.Contains(c.gameObject))
                {
                    c.gameObject.GetComponent<Enemy>().HandleHit(GetDamage(), GetKnockback());
                    if (!isMelee) { Destroy(gameObject); }
                    else { hits.Add(c.gameObject); }
                }

            });
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        /*
        if (!isMelee)
        {
            if (collision.CompareTag("Player")) { Destroy(gameObject); }
            // Do not destroy on collision with self
            if (collision.CompareTag("Enemy") && collision.gameObject != creator)
            {
                collision.gameObject.GetComponent<Enemy>().HandleHit(GetDamage(), GetKnockback());
                Destroy(gameObject);

            }
            if (collision.CompareTag("Wall")) { Destroy(gameObject); }
        }
        else 
        {
            if (collision.CompareTag("Enemy") && collision.gameObject != creator && !hits.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Enemy>().HandleHit(GetDamage(), GetKnockback());
                hits.Add(collision.gameObject);
            }
        }
        */
        if (!isMelee && collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    public void Create(GameObject _creator, Sprite _sprite, Vector2 _size, LayerMask _mask, Vector2 _position, Vector2 _velocity, int _damage, float _knockback, float _duration, bool _isMelee)
    {
        creator = _creator;
        velocity = _velocity;
        damage = _damage;
        knockback = _knockback;
        duration = _duration;
        isMelee = _isMelee;

        transform.position = _position;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _sprite;
        spriteRenderer.sortingLayerName = "Projectile";

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.size = _size;
        boxCollider.isTrigger = true;

        filter.useLayerMask = true;
        filter.layerMask = _mask;


        projRigidbody = gameObject.AddComponent<Rigidbody2D>();
        projRigidbody.gravityScale = 0;
        projRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        projRigidbody.freezeRotation = true;

        gameObject.tag = "Projectile";
        gameObject.name = "Projectile";

        gameObject.transform.SetParent(creator.transform);
    }

    public int GetDamage() { return damage; }
    public GameObject GetCreator() { return creator; }
    public Vector2 GetKnockback() { return velocity.normalized * knockback; }
    


}
