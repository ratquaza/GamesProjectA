using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D target;

    //camera smoothness: 
    //lower number  -> smoother
    //higher number -> more robotic
    [SerializeField] private float smoothSpeed = 5f; 
    [SerializeField] private float velocityTrackingFactor = 0f;
    [SerializeField] private float mouseTrackingFactor = 0f;

    void Start()
    {
        transform.position = target.position;
    }

    void Update()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + target.velocity * velocityTrackingFactor;
        desiredPosition = Vector3.Lerp(desiredPosition, Camera.main.ScreenToWorldPoint(Input.mousePosition), mouseTrackingFactor);
        transform.position = Vector3.Lerp(transform.position, desiredPosition - Vector3.forward * 500, smoothSpeed * Time.deltaTime);
    }
}