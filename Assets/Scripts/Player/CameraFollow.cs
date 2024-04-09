using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    //camera smoothness: 
    //lower number  -> smoother
    //higher number -> more robotic
    [SerializeField] private float smoothSpeed = 5f; 
    [SerializeField] private float mouseTrackingFactor = 0f;

    private Vector2? center = null;
    private Vector2? bounds = null;

    void Start()
    {
        transform.position = target.position;
    }

    void Update()
    {
        if (target == null) return;
        Vector2 desiredPosition = target.position;
        desiredPosition = Vector2.Lerp(desiredPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseTrackingFactor);
        if (center != null && bounds != null) {
            float height = Camera.main.orthographicSize * 2;
            float width = height * Camera.main.aspect;

            desiredPosition = new Vector2( 
                Math.Clamp(desiredPosition.x, center.Value.x - bounds.Value.x/2 + width/2, center.Value.x + bounds.Value.x/2 - width/2), 
                Math.Clamp(desiredPosition.y, center.Value.y - bounds.Value.y/2 + height/2, center.Value.y + bounds.Value.y/2 - height/2)
            );
        }
        transform.position = Vector2.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void UpdateTarget(Transform target)
    {
        this.target = target;
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
}