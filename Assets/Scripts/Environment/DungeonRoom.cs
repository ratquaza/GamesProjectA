using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonRoom : MonoBehaviour
{
    // [SerializeField] private BoxCollider2D boundsCollider;
    // [SerializeField] private Vector2 boundsOffset;
    // [SerializeField] private CameraFollow cameraScript;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private BoxCollider2D triggerCollider;
    [SerializeField] public Quadrant[] borders = new Quadrant[0];
    [SerializeField] public EnemySpawnTable spawns;

    public event Action<PlayerLiving> onPlayerEnter;

    public Vector2Int gridPosition;
    public int width { 
        get => (int) Math.Ceiling((double) wallTilemap.cellBounds.size.x/DungeonManager.ROOM_WIDTH); 
    }

    public int height { 
        get => (int) Math.Ceiling((double) wallTilemap.cellBounds.size.y/DungeonManager.ROOM_HEIGHT);
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
        Vector3 quadPos = transform.position + new Vector3(quad.position.x * DungeonManager.ROOM_WIDTH, quad.position.y * DungeonManager.ROOM_HEIGHT);
        switch (exit)
        {
            case ExitDirection.North: return quadPos + new Vector3(DungeonManager.ROOM_WIDTH/2f, DungeonManager.ROOM_HEIGHT - .5f);
            case ExitDirection.East: return quadPos + new Vector3(DungeonManager.ROOM_WIDTH - .5f, DungeonManager.ROOM_HEIGHT/2f);
            case ExitDirection.South: return quadPos + new Vector3(DungeonManager.ROOM_WIDTH/2f, .5f);
            default: return quadPos + new Vector3(.5f, DungeonManager.ROOM_HEIGHT/2f);
        }
    }

    public void SpawnEnemies(bool startDisabled = false)
    {
        if (spawns == null) return;
        foreach (EnemySpawnTable.LocatedSet locatedSet in spawns.sets)
        {
            GameObject enemy = Instantiate(locatedSet.set.GetSpawn().enemyPrefab.gameObject, transform);
            enemy.transform.localPosition = locatedSet.position;
            if (startDisabled)
            {
                enemy.SetActive(false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D data)
    {
        PlayerLiving player = data.GetComponent<PlayerLiving>();
        if (player == null) return;
        CameraFollow camera = Camera.main.GetComponent<CameraFollow>();
        camera.SetBounds((Vector2) transform.position + triggerCollider.offset, triggerCollider.size + Vector2.one);
        onPlayerEnter?.Invoke(player);
    }

    [ExecuteInEditMode]
    void OnDrawGizmosSelected()
    {
        if (wallTilemap == null) return;
        foreach (Quadrant quad in borders)
        {
            Vector3 offset = new Vector3(quad.position.x * DungeonManager.ROOM_WIDTH, quad.position.y * DungeonManager.ROOM_HEIGHT);

            Gizmos.color = Color.white;
            Gizmos.DrawWireCube(
                transform.position + offset + new Vector3(DungeonManager.ROOM_WIDTH/2f, DungeonManager.ROOM_HEIGHT/2f), 
                new Vector2(DungeonManager.ROOM_WIDTH, DungeonManager.ROOM_HEIGHT)
            );
            
            Handles.Label(transform.position + offset + new Vector3(.2f, .5f), $"Quadrant {quad.position.x} {quad.position.y}");
            Gizmos.color = Color.blue;
            if (quad.northExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.North), transform.localScale);
            if (quad.eastExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.East), transform.localScale);
            if (quad.southExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.South), transform.localScale);
            if (quad.westExit) Gizmos.DrawCube(ExitPosition(quad, ExitDirection.West), transform.localScale);
        }

        if (spawns != null)
        {
            for (int i = 0; i < spawns.sets.Length; i++)
            {
                EnemySpawnTable.LocatedSet location = spawns.sets[i];

                Gizmos.color = Color.red;
                Gizmos.DrawCube(transform.TransformPoint(location.position), Vector3.one * 1f); 
                Gizmos.color = Color.white;
                Handles.Label(transform.TransformPoint(location.position - new Vector2(0.25f, .75f)), "#" + i);
            }
        }
    }
}