using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private int roomWidth = 15;
    [SerializeField] private int roomHeight = 9;
    [SerializeField] private GameObject[] roomPool;
    [SerializeField] private int maxRooms = 20;
    private Grid grid;
    private GameObject[,] currentDungeon;
    
    void Start()
    {
        grid = GetComponent<Grid>();
        currentDungeon = new GameObject[maxRooms/2, maxRooms/2];

        int x = UnityEngine.Random.Range(0, maxRooms/2);
        int y = UnityEngine.Random.Range(0, maxRooms/2);

        GameObject spawnRoom = CreateRandomRoom();
        spawnRoom.transform.localPosition = LocalPositionFromGrid(new Vector2Int(x, y));
        spawnRoom.name = $"{x} {y} SpawnRoom";
        currentDungeon[x, y] = spawnRoom;

        maxRooms--;
        List<GameObject> rooms = FillExits(spawnRoom, 8, true).ToList<GameObject>();
        while (maxRooms > 0 && rooms.Count > 0)
        {
            List<GameObject> queue = new List<GameObject>();
            foreach (GameObject room in rooms)
            {
                queue.AddRange(FillExits(room, 8));
            }
            rooms.AddRange(queue);
            maxRooms -= queue.Count;
        }
    }

    Vector3 LocalPositionFromGrid(Vector2Int pos)
    {
        return new Vector3(pos.x * roomWidth, pos.y * roomHeight, 0);
    }

    Vector2Int GridPositionFromLocal(Vector2 room)
    {
        return new Vector2Int(
            (int) Math.Ceiling(room.x/roomWidth), 
            (int) Math.Ceiling(room.y/roomHeight)
        );
    }

    Vector2Int GetGridSize(BoundsInt roomBounds)
    {
        return new Vector2Int(
            (int) Math.Ceiling(roomBounds.size.x/(double)roomWidth),
            (int) Math.Ceiling(roomBounds.size.y/(double)roomHeight)
        );
    }

    GameObject CreateRandomRoom(Predicate<GameObject> predicate = null)
    {
        GameObject selectedRoom;
        if (predicate == null)
        {
            selectedRoom = roomPool[UnityEngine.Random.Range(0, roomPool.Length)];
        }
        else
        {
            GameObject[] validRooms = roomPool.Where(predicate.Invoke).ToArray();
            if (validRooms.Length == 0) return null;
            selectedRoom = validRooms[UnityEngine.Random.Range(0, validRooms.Length)];
        }

        selectedRoom = Instantiate(selectedRoom, grid.transform);
        Tilemap tilemap = selectedRoom.transform.Find("Walls").GetComponent<Tilemap>();
        tilemap.CompressBounds();

        return selectedRoom;
    }

    GameObject[] FillExits(GameObject baseRoom, int generanteChance = 1, bool guaranteeOneRoom = false)
    {
        generanteChance = Math.Max(1, generanteChance);
        Transform baseRoomWall = baseRoom.transform.Find("Walls");
        Vector2Int baseRoomGridPos = GridPositionFromLocal(baseRoom.transform.localPosition);
        
        bool nextGuaranteed = false;

        List<GameObject> generatedRooms = new List<GameObject>();
        foreach (Transform exit in baseRoomWall)
        {
            if (!nextGuaranteed && generanteChance > 1 && UnityEngine.Random.Range(1, generanteChance) != 1)
            {
                if (guaranteeOneRoom)
                {
                    nextGuaranteed = true;
                    guaranteeOneRoom = false;
                }
                continue;
            }
            ExitDirection direction = ExitDirectionExtensions.From(exit.name);
            Vector2Int attachedRoomGridPos = baseRoomGridPos + direction.ToVector();
            if (attachedRoomGridPos.x >= maxRooms/2 || attachedRoomGridPos.x < 0 || attachedRoomGridPos.y >= maxRooms/2 || attachedRoomGridPos.y < 0) continue;
            if (currentDungeon[attachedRoomGridPos.x, attachedRoomGridPos.y] != null) continue;

            GameObject attachedRoom = CreateRandomRoom(g => g.transform.Find("Walls").Find(direction.Opposite().ToString()) != null);
            if (attachedRoom == null) continue;
            if (nextGuaranteed) nextGuaranteed = false;

            AttachRoomTo(baseRoom, attachedRoom, direction);
            attachedRoom.name = attachedRoomGridPos.ToString();
            currentDungeon[attachedRoomGridPos.x, attachedRoomGridPos.y] = attachedRoom;

            generatedRooms.Add(attachedRoom);
        }

        return generatedRooms.ToArray();
    }

    void AttachRoomTo(GameObject baseRoom, GameObject attacher, ExitDirection baseFrom)
    {
        Transform baseWalls = baseRoom.transform.Find("Walls");
        Transform attachWalls = attacher.transform.Find("Walls");
        Tilemap baseTM = baseWalls.GetComponent<Tilemap>();
        Tilemap attachTM = attachWalls.GetComponent<Tilemap>();

        Vector2Int baseGrid = GridPositionFromLocal(baseRoom.transform.localPosition);
        Vector2Int attacherGrid = baseGrid + baseFrom.ToVector();
        attacher.transform.localPosition = LocalPositionFromGrid(attacherGrid);
        currentDungeon[attacherGrid.x, attacherGrid.y] = attacher;

        baseTM.SetTile(Vector3Int.FloorToInt(baseWalls.transform.Find(baseFrom.ToString()).localPosition), null);
        Destroy(baseWalls.transform.Find(baseFrom.ToString()).gameObject);
        attachTM.SetTile(Vector3Int.FloorToInt(attachWalls.transform.Find(baseFrom.Opposite().ToString()).localPosition), null);
        Destroy(attachWalls.transform.Find(baseFrom.Opposite().ToString()).gameObject);
    }

    void Update()
    {

    }
}

public enum ExitDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public static class ExitDirectionExtensions
{
    
    private static readonly Vector2Int[] vectors = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    public static ExitDirection From(int value)
    {
        value %= 4;
        return (ExitDirection) value;
    }

    public static ExitDirection From(string value)
    {
        switch (value)
        {
            case "North":
                return (ExitDirection) 0;
            case "East":
                return (ExitDirection) 1;
            case "South":
                return (ExitDirection) 2;
            case "West":
                return (ExitDirection) 3;
            default:
                throw new ArgumentException("No direction with name " + value);
        }
    }

    public static Vector2Int ToVector(this ExitDirection exit)
    {
        return vectors[(int) exit];
    }

    public static ExitDirection Opposite(this ExitDirection exit)
    {
        return From((int) exit + 2);
    }
}