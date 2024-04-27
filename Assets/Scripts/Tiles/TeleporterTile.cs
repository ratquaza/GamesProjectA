using UnityEngine;

public class TeleporterTile : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;
    private Collider2D teleportTargetCollider;

    private void Start()
    {
        teleportTargetCollider = teleportTarget.gameObject.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Rigidbody2D>() != null)
        {
            if(Vector2.Distance(other.transform.position, transform.position) > 0.2f)
            other.transform.position = new Vector3(teleportTarget.position.x, teleportTarget.position.y, other.transform.position.z);
        }
    }
}
