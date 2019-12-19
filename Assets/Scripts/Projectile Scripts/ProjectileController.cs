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

    void FixedUpdate()
    {
        projRigidbody.velocity = velocity;
        if ( timer >=  duration) { Destroy(gameObject); }
        else { timer += Time.fixedDeltaTime; }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isMelee)
        {
            if (collision.CompareTag("Player")) { Destroy(gameObject); }
            // Do not destroy on collision with self
            if (collision.CompareTag("Enemy") && collision.gameObject != creator) { Destroy(gameObject); }
            if (collision.CompareTag("Wall")) { Destroy(gameObject); }
        }
    }
    public void Create(GameObject _creator, Sprite _sprite, Vector2 _size, LayerMask _mask, Vector2 _position, Vector2 _velocity, int _damage, float _knockback, float _duration)
    {
        creator = _creator;
        velocity = _velocity;
        damage = _damage;
        knockback = _knockback;
        duration = _duration;
        isMelee = false;

        transform.position = _position;

        spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _sprite;

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.size = _size;
        boxCollider.isTrigger = true;

        gameObject.layer = _mask;

        projRigidbody = gameObject.AddComponent<Rigidbody2D>();
        projRigidbody.gravityScale = 0;
        projRigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        projRigidbody.freezeRotation = true;

        gameObject.tag = "Projectile";
        gameObject.name = "Projectile";

        gameObject.transform.SetParent(creator.transform);
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

        boxCollider = gameObject.AddComponent<BoxCollider2D>();
        boxCollider.size = _size;
        boxCollider.isTrigger = true;

        gameObject.layer = _mask;

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
