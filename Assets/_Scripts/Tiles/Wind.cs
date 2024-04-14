using UnityEngine;

public class Wind : MonoBehaviour
{
    [SerializeField] private Vector2 windVelocity = new Vector2(0f, -5f);

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb2d = other.gameObject.GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity += windVelocity * Time.deltaTime;
        }
    }
}
