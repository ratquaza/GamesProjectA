using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTile : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collider)
    {
        PlayerLiving player = collider.GetComponent<PlayerLiving>();
        if (player == null) return;
        player.TakeDamage(15);

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Vector2 point = collider.ClosestPoint(transform.position);

        rb.velocity += (((Vector2) player.transform.position) - point).normalized * 5f;
    }
}
