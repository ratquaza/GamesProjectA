using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boundsCollider;
    [SerializeField] private CameraFollow cameraScript;

    void OnTriggerEnter2D(Collider2D data)
    {
        PlayerLiving player = data.GetComponent<PlayerLiving>();
        if (player == null) return;
        cameraScript.SetBounds(transform.TransformPoint(boundsCollider.offset), boundsCollider.size);
    }
}
