using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    private PlayerMovement player;
    private float maxSpeed;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerMovement currPlayer = coll.GetComponent<PlayerMovement>();
        if (currPlayer == null) return;
        player = currPlayer;
        maxSpeed = currPlayer.GetMaxSpeed();
        currPlayer.SetMaxSpeed(maxSpeed/3f);
        currPlayer.GetComponent<SpriteRenderer>().color = new Color(0.37f, 0.37f, 1);
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        PlayerMovement currPlayer = coll.GetComponent<PlayerMovement>();
        if (currPlayer == null) return;
        currPlayer.SetMaxSpeed(maxSpeed);
        currPlayer.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
}
