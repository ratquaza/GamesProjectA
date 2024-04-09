using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boundsCollider;
    [SerializeField] private Vector2 boundsOffset;
    [SerializeField] private CameraFollow cameraScript;

    void OnTriggerStay2D(Collider2D data)
    {
        PlayerLiving player = data.GetComponent<PlayerLiving>();
        if (player == null) return;
        cameraScript.SetBounds(transform.TransformPoint(boundsCollider.offset), boundsCollider.size + boundsOffset);
    }

    [ExecuteInEditMode]
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.TransformPoint(boundsCollider.offset), boundsCollider.size);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.TransformPoint(boundsCollider.offset), boundsCollider.size + boundsOffset);
    }
}
