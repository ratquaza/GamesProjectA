using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Rigidbody2D target;

    //camera smoothness: 
    //lower number  -> smoother
    //higher number -> more robotic
    [SerializeField] private float smoothSpeed = 3f; 
    [SerializeField] private float velocityTrackingFactor = 0f;

    void Update()
    {
        if (target == null) return;
        Vector3 desiredPosition = target.transform.position + (Vector3) target.velocity * velocityTrackingFactor;
        transform.position = Vector3.Lerp(transform.position, desiredPosition - Vector3.forward * 500, smoothSpeed * Time.deltaTime);
    }
}