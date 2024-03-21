using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    //camera smoothness: 
    //lower number  -> smoother
    //higher number -> more robotic
    [SerializeField] private float smoothSpeed = 0.01f; 


    [SerializeField] private Vector3 offset; //offset of camera from target

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
