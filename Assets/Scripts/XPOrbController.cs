using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPOrbController : MonoBehaviour
{
    public int XPAmount;
    GameObject player;
    Rigidbody2D rb;
    BoxCollider2D bc;

    public void Start()
    {
        XPAmount = 1;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
    }

    public void FixedUpdate()
    {
        Vector2 playerDistance = new Vector2(transform.position.x - player.transform.position.x, transform.position.y - player.transform.position.y);
        if (playerDistance.magnitude <= 3) {
            rb.velocity = playerDistance.normalized * -15;
        }
        if (Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("Player")))
        {
            player.GetComponent<PlayerController>().stat_totalXP++;
            Debug.Log(player.GetComponent<PlayerController>().stat_totalXP);
            Destroy(gameObject);
        }

    }
}
