using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    private PlayerMovement player;
    private float moveSpeed;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerMovement currPlayer = coll.GetComponent<PlayerMovement>();
        if (currPlayer == null) return;
        player = currPlayer;
        moveSpeed = currPlayer.GetMoveSpeed();
        currPlayer.SetMoveSpeed(moveSpeed/3f);
        currPlayer.GetComponent<SpriteRenderer>().color = new Color(0.37f, 0.37f, 1);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        PlayerMovement currPlayer = coll.GetComponent<PlayerMovement>();
        if (currPlayer == null) return;
        currPlayer.SetMoveSpeed(moveSpeed);
        currPlayer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
