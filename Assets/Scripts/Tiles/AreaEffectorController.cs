using UnityEngine;

public class AreaEffectorController : MonoBehaviour
{
    public AreaEffector2D areaEffector;

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
}
