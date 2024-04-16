using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTile : MonoBehaviour
{
    private Vector2 direction;
    private Rigidbody2D player;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerLiving currPlayer = coll.GetComponent<PlayerLiving>();
        if (currPlayer == null) return;
        player = currPlayer.GetComponent<Rigidbody2D>();
        direction = player.velocity.normalized;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        PlayerLiving currPlayer = coll.GetComponent<PlayerLiving>();
        if (currPlayer == null) return;
        if (player != null) player.velocity = direction * 10f;
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        PlayerLiving currPlayer = coll.GetComponent<PlayerLiving>();
        if (currPlayer == null) return;
        player = null;
    }
}
