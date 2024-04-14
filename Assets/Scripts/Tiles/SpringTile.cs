using System.Collections;
using UnityEngine;

public class SpringTile : MonoBehaviour
{
    private PlayerMovement playerMovement;

    [SerializeField] private Vector2 SpringVelocity = new Vector2(0f, 50f);

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        Rigidbody2D rb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity += SpringVelocity;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            playerMovement = other.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;
        }
    }
}
