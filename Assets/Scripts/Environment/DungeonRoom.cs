using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonRoom : MonoBehaviour
{
    [SerializeField] private BoxCollider2D boundsCollider;
    [SerializeField] private Vector2 boundsOffset;
    [SerializeField] private CameraFollow cameraScript;
    
    private List<Enemy> enemies = new List<Enemy>();
    private List<Enemy> toRemove = new List<Enemy>();

    void Start()
    {
        foreach (Transform trans in transform)
        {
            Enemy enemy = trans.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemies.Add(enemy);
                enemy.gameObject.SetActive(false);
                enemy.onHealthChange += (hp) => 
                {
                    if (hp <= 0) toRemove.Add(enemy);
                }; 
            }
        }
    }

    void OnTriggerStay2D(Collider2D data)
    {
        PlayerLiving player = data.GetComponent<PlayerLiving>();
        if (player == null) return;
        cameraScript.SetBounds(transform.TransformPoint(boundsCollider.offset), boundsCollider.size + boundsOffset);
        if (toRemove.Count > 0)
        {
            enemies.RemoveAll((e) => toRemove.Contains(e));
            toRemove.Clear();

        }
        foreach (var enemy in enemies) enemy.gameObject.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D data)
    {
        PlayerLiving player = data.GetComponent<PlayerLiving>();
        if (player == null) return;
        foreach (var enemy in enemies) enemy.gameObject.SetActive(false);
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
