using UnityEngine;

public class TeleporterTile : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;
    private Collider2D teleportTargetCollider;
    private bool isTeleporting = false;

    private void Start()
    {
        teleportTargetCollider = teleportTarget.gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTeleporting && other.GetComponent<Rigidbody2D>() != null)
        {
            float distance = Vector2.Distance(other.transform.position, transform.position);
            if (distance > 0.5f)
            {
                isTeleporting = true;
                other.transform.position = new Vector3(teleportTarget.position.x, teleportTarget.position.y, other.transform.position.z);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isTeleporting && other.GetComponent<Rigidbody2D>() != null)
        {
            isTeleporting = false;
        }
    }
}
