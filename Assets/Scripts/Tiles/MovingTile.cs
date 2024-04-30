using UnityEngine;
using UnityEngine.UI;

public class MovingTile : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    private float originalSpeed;
    private int currentWaypointIndex = 0;
    [SerializeField] private float accelerationRate = 1.0f;

    public enum TileState
    {
        MovementEnabled,
        MovementDisabled,
        FastModeEnabled,
        SlowModeEnabled,
        AccelerationModeEnabled
    }

    public TileState currentState = TileState.MovementEnabled;

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
            case TileState.FastModeEnabled:
                MoveWithSpeedModifier(2f);
                break;
            case TileState.SlowModeEnabled:
                MoveWithSpeedModifier(0.5f);
                break;
            case TileState.AccelerationModeEnabled:
                MoveWithAcceleration();
                break;
        }
    }

    public void ChangeState(TileState newState)
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

    void MoveWithSpeedModifier(float speedModifier)
    {
        if (waypoints.Length == 0) return;

        float currentSpeed = speed * speedModifier;
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, currentSpeed * Time.deltaTime);

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
