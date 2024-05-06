using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //camera smoothness: 
    //lower number  -> smoother
    //higher number -> more robotic
    [SerializeField] private float smoothSpeed = 5f; 
    [SerializeField] private float mouseTrackingFactor = 0f;
    private PlayerLiving player { get => PlayerLiving.Instance; }

    private bool trackMouse = true;

    private Vector2? center = null;
    private Vector2? bounds = null;

    void Start()
    {
        if (player != null) transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (player == null) return;

        Vector2 desiredPosition = player.transform.position;
        if (trackMouse) desiredPosition = Vector2.Lerp(desiredPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseTrackingFactor);

        if (center != null && bounds != null) {
            float height = Camera.main.orthographicSize * 2;
            float width = height * Camera.main.aspect;

            float xMin = center.Value.x - bounds.Value.x/2 + width/2;
            float xMax = center.Value.x + bounds.Value.x/2 - width/2;
            float yMin = center.Value.y - bounds.Value.y/2 + height/2;
            float yMax = center.Value.y + bounds.Value.y/2 - height/2;

            float newX = xMin < xMax ? Math.Clamp(desiredPosition.x, xMin, xMax) : center.Value.x;
            float newY = yMin < yMax ? Math.Clamp(desiredPosition.y, yMin, yMax) : center.Value.y;

            desiredPosition = new Vector2(newX, newY);
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(desiredPosition.x, desiredPosition.y, transform.position.z), smoothSpeed * Time.deltaTime);
    }

    public void SetBounds(Vector2 center, Vector2 bounds)
    {
        this.center = center;
        this.bounds = bounds;
    }

    public void ClearBounds()
    {
        this.center = null;
        this.bounds = null;
    }

    public void SetFollowMouse(bool value)
    {
        trackMouse = value;
    }
}