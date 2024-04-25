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
        switch (direction)
        {
            case Direction.Up:
                areaEffector.forceAngle = 90f;
                break;
            case Direction.Down:
                areaEffector.forceAngle = 270f;
                break;
            case Direction.Left:
                areaEffector.forceAngle = 180f;
                break;
            case Direction.Right:
                areaEffector.forceAngle = 0f;
                break;
        }
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
