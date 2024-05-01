using UnityEngine;

public class RepelMouse : MonoBehaviour
{
    [SerializeField] private float pushForceMultiplier = 500000f;
    private PlayerActions playerActions;

    private void Awake()
    {
        playerActions = PlayerLiving.Instance.actions;

        playerActions.Movement.RepelMouse.performed += ctx => RepelFromMouse();
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    private void RepelFromMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        Vector3 direction = transform.position - mousePosition;
        float distance = direction.magnitude;
        direction.Normalize();
        float pushForce = pushForceMultiplier / (distance + 1f); //avoid div by0
        Vector2 velocity = direction * pushForce;
        GetComponent<Rigidbody2D>().velocity = velocity;
    }
}
