using UnityEngine;

public class BlinkTarget : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private bool canTeleport = true;
    private Vector3 lastValidLocation;

    public bool GetCanTeleport()
    {
        return canTeleport;
    }

    public Vector3 GetLastValidLocation()
    {
        return lastValidLocation;
    }

    void Start()
    {
        // Ensure spriteRenderer is assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Set initial sprite color to transparent green
        SetSpriteColor(Color.green);
    }

    // Update is called once per frame
    void Update()
    {
        // If canTeleport is true, set sprite color to transparent green
        if (canTeleport)
        {
            SetSpriteColor(Color.green);
            lastValidLocation = transform.position;
        }
    }

    // Handle collision with wall layer
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Set sprite color to transparent red
            SetSpriteColor(Color.red);
            // Set canTeleport to false
            canTeleport = false;
        }
    }

    // Handle exit from collision with wall layer
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Set canTeleport to true
            canTeleport = true;
        }
    }

    // Method to set sprite color
    private void SetSpriteColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
