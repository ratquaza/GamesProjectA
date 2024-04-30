using UnityEngine;

public class AreaEffectorController : MonoBehaviour
{
    public AreaEffector2D areaEffector;
    private float originalMagnitude;


    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public Direction direction;

    void Start()
    {
        SetDirection();
        originalMagnitude = areaEffector.forceMagnitude;
    }

    void SetDirection()
    {
        Quaternion newRotation = Quaternion.identity;

        switch (direction)
        {
            case Direction.Up:
                newRotation = Quaternion.Euler(0f, 0f, 0f);
                break;
            case Direction.Down:
                newRotation = Quaternion.Euler(0f, 0f, 180f);
                break;
            case Direction.Left:
                newRotation = Quaternion.Euler(0f, 0f, 90f);
                break;
            case Direction.Right:
                newRotation = Quaternion.Euler(0f, 0f, -90f);
                break;
        }

        // Set rotation of the parent GameObject
        transform.rotation = newRotation;
    }


    void OnValidate()
    {
        SetDirection();
    }

    public void ChangeDirection(Direction newDirection)
    {
        direction = newDirection;
        SetDirection();
    }


    [SerializeField] private bool accelerateMode = false; 
    [SerializeField] private float acceleration = 9.8f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (accelerateMode)
        {
            areaEffector.forceMagnitude += acceleration * Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        areaEffector.forceMagnitude = originalMagnitude;    
    }
}
