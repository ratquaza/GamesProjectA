using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    // [SerializeField] private BoxCollider2D boundsCollider;
    // [SerializeField] private Vector2 boundsOffset;
    // [SerializeField] private CameraFollow cameraScript;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] public Quadrant[] borders = new Quadrant[0];
    public Vector2Int gridPosition;

    public int width { 
        get => (int) Math.Ceiling((double) wallTilemap.cellBounds.size.x/DungeonGenerator.ROOM_WIDTH); 
    }

    public int height { 
        get => (int) Math.Ceiling((double) wallTilemap.cellBounds.size.y/DungeonGenerator.ROOM_HEIGHT);
    }

    public Vector2Int area {
        get => new Vector2Int(width, height);
    }

    public Quadrant? GetQuadrant(Vector2Int pos)
    {
        return GetQuadrant(pos.x, pos.y);
    }

    public Quadrant? GetQuadrant(int x, int y)
    {
        try {
            return borders.First((q) => q.position.x == x && q.position.y == y);
        } catch (InvalidOperationException)
        {
            return null;
        }
    }

    public Quadrant[] GetWithExit(ExitDirection direction) => borders.Where((e) => e.CanExit(direction)).ToArray();

    public void ClearExit(Quadrant quad, ExitDirection exit) => wallTilemap.SetTile(wallTilemap.WorldToCell(ExitPosition(quad, exit)), null);

    public Vector3 ExitPosition(Quadrant quad, ExitDirection exit)
    {
        Vector3 quadPos = transform.position + new Vector3(quad.position.x * DungeonGenerator.ROOM_WIDTH, quad.position.y * DungeonGenerator.ROOM_HEIGHT);
        switch (exit)
        {
            case ExitDirection.North: return quadPos + new Vector3(DungeonGenerator.ROOM_WIDTH/2f, DungeonGenerator.ROOM_HEIGHT - .5f);
            case ExitDirection.East: return quadPos + new Vector3(DungeonGenerator.ROOM_WIDTH - .5f, DungeonGenerator.ROOM_HEIGHT/2f);
            case ExitDirection.South: return quadPos + new Vector3(DungeonGenerator.ROOM_WIDTH/2f, .5f);
            default: return quadPos + new Vector3(.5f, DungeonGenerator.ROOM_HEIGHT/2f);
        }
    }
    
    // private List<Enemy> enemies = new List<Enemy>();
    // private List<Enemy> toRemove = new List<Enemy>();

    // void Start()
    // {
    //     foreach (Transform trans in transform)
    //     {
    //         Enemy enemy = trans.GetComponent<Enemy>();
    //         if (enemy != null)
    //         {
    //             enemies.Add(enemy);
    //             enemy.gameObject.SetActive(false);
    //             enemy.onHealthChange += (hp) => 
    //             {
    //                 if (hp <= 0) toRemove.Add(enemy);
    //             }; 
    //         }
    //     }
    // }

    // void OnTriggerStay2D(Collider2D data)
    // {
    //     PlayerLiving player = data.GetComponent<PlayerLiving>();
    //     if (player == null) return;
    //     cameraScript.SetBounds(transform.TransformPoint(boundsCollider.offset), boundsCollider.size + boundsOffset);
    //     if (toRemove.Count > 0)
    //     {
    //         enemies.RemoveAll((e) => toRemove.Contains(e));
    //         toRemove.Clear();

    //     }
    //     foreach (var enemy in enemies) enemy.gameObject.SetActive(true);
    // }

    // void OnTriggerExit2D(Collider2D data)
    // {
    //     PlayerLiving player = data.GetComponent<PlayerLiving>();
    //     if (player == null) return;
    //     foreach (var enemy in enemies) enemy.gameObject.SetActive(false);
    // }

    [ExecuteInEditMode]
    void OnDrawGizmosSelected()
    {
        if (wallTilemap == null) return;
        foreach (Quadrant quad in borders)
        {
            Vector3 offset = new Vector3(quad.position.x * DungeonGenerator.ROOM_WIDTH, quad.position.y * DungeonGenerator.ROOM_HEIGHT);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(
                transform.position + offset + new Vector3(DungeonGenerator.ROOM_WIDTH/2f, DungeonGenerator.ROOM_HEIGHT/2f), 
                new Vector2(DungeonGenerator.ROOM_WIDTH, DungeonGenerator.ROOM_HEIGHT)
            );

            Gizmos.color = Color.blue;
            if (quad.northExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.North), transform.localScale);
            if (quad.eastExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.East), transform.localScale);
            if (quad.southExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.South), transform.localScale);
            if (quad.westExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.West), transform.localScale);
        }
    }
}