using UnityEngine;

public class MovingTile : MonoBehaviour
{
    public Transform[] waypoints;
    public Transform rotationPivot;
    private float originalSpeed;
    private int currentWaypointIndex = 0;

    public TileState currentState;

    public enum TileState
    {
        MovementEnabled,
        MovementDisabled,
        AccelerationModeEnabled,
        RotateEnabled
    }

    [Header("Movement Enabled Settings")]
    [SerializeField] private float speed = 2f;

    [Header("Rotation Enabled Settings")]
    [SerializeField] private float rotationSpeed = 10f;

    [Header("Acceleration Mode Settings")]
    [SerializeField] private float accelerationRate = 1.0f;



    

    private void Start()
    {
        originalSpeed = speed;

        if (waypoints.Length > 0) transform.position = waypoints[0].position;
    }

    void Update()
    {
        switch (currentState)
        {
            case TileState.MovementEnabled:
                Move();
                break;
            case TileState.MovementDisabled:
                // Movement Disabled -> Do nothing
                break;
            case TileState.AccelerationModeEnabled:
                MoveWithAcceleration();
                break;
            case TileState.RotateEnabled: // Handle rotation state
                Rotate(rotationPivot);
                break;
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetState(TileState newState)
    {
        currentState = newState;
    }

    void Move()
    {
        if (waypoints.Length == 0) return;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if the platform has reached the current waypoint (0.1f: arbitrary value)
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Move to next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void MoveWithAcceleration()
    {

        speed += accelerationRate * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, speed * Time.deltaTime);

        // Check if the platform has reached the current waypoint (0.1f: arbitrary value)
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            // Move to next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            // Reset to original speed
            speed = originalSpeed;
        }
        Debug.Log(speed);
    }

    void Rotate(Transform pivot)
    {
        // Rotate the parent's transform
        if (transform.parent != null)
        {
            transform.parent.RotateAround(pivot.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

    // Draw debugging lines for the platform waypoints path
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (waypoints.Length > 1)
        {
            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
    }
}
