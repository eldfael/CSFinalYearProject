using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPPackController : MonoBehaviour
{
    public int hpAmount = 2;
    GameObject playerObject;
    Rigidbody2D rb;
    Vector2 playerDistance;

    float timer;
    float aliveTime = 6f;

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (timer >= aliveTime) { Destroy(gameObject); }
        else { timer += Time.fixedDeltaTime; }

        playerDistance = new Vector2(playerObject.transform.position.x - transform.position.x, playerObject.transform.position.y - transform.position.y);

        if (playerDistance.magnitude <= 1.5f)
        {
            rb.velocity = playerDistance.normalized * 10f;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        if (Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("Player") | LayerMask.GetMask("PlayerRolling")))
        {
            playerObject.GetComponent<PlayerController>().HandleHPGain(hpAmount);
            Destroy(gameObject);
        }
    }
}
