using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    public static readonly int ROOM_WIDTH = 15;
    public static readonly int ROOM_HEIGHT = 9;

    [SerializeField] private GameObject[] suppliedRoomPool;
    [SerializeField] private int maxRooms = 20;

    private DungeonRoom[] roomPool;
    private Grid grid;
    private DungeonRoom[,] currentDungeon;
    
    void Start()
    {
        roomPool = suppliedRoomPool.Select(room => new DungeonRoom(room.transform)).ToArray();

        grid = GetComponent<Grid>();
        currentDungeon = new DungeonRoom[maxRooms, maxRooms];

        int x = maxRooms/2;
        int y = x;

        DungeonRoom spawnRoom = CreateRandomRoom();
        spawnRoom.RoomObject.transform.localPosition = LocalPositionFromGrid(new Vector2Int(x, y));
        spawnRoom.RoomObject.name = $"{x} {y} SpawnRoom";
        currentDungeon[x, y] = spawnRoom;

        FillExits(spawnRoom, 1);
    }

    Vector3 LocalPositionFromGrid(Vector2Int pos)
    {
        return new Vector3(pos.x * ROOM_WIDTH, pos.y * ROOM_HEIGHT, 0);
    }

    Vector2Int GridPositionFromLocal(Vector2 room)
    {
        return new Vector2Int(
            (int) Math.Ceiling(room.x/ROOM_WIDTH), 
            (int) Math.Ceiling(room.y/ROOM_HEIGHT)
        );
    }

    DungeonRoom CreateRandomRoom(Predicate<DungeonRoom> predicate = null)
    {
        DungeonRoom selectedRoom;
        if (predicate == null)
        {
            selectedRoom = roomPool[UnityEngine.Random.Range(0, roomPool.Length)];
        }
        else
        {
            DungeonRoom[] validRooms = roomPool.Where(predicate.Invoke).ToArray();
            if (validRooms.Length == 0) return null;
            selectedRoom = validRooms[UnityEngine.Random.Range(0, validRooms.Length)];
        }

        selectedRoom = new DungeonRoom(Instantiate(selectedRoom.RoomObject, grid.transform));
        Tilemap tilemap = selectedRoom.WallsObject.GetComponent<Tilemap>();
        tilemap.CompressBounds();

        return selectedRoom;
    }

    DungeonRoom[] FillExits(DungeonRoom baseRoom, int generanteChance = 1)
    {
        generanteChance = Math.Max(1, generanteChance);
        Transform baseRoomWall = baseRoom.WallsObject;
        Vector2Int baseRoomGridPos = GridPositionFromLocal(baseRoom.RoomObject.localPosition);

        List<DungeonRoom> generatedRooms = new List<DungeonRoom>();
        Transform[] exits = baseRoom.GetExits();
        foreach (Transform exit in exits)
        {
            if (generanteChance > 1 && UnityEngine.Random.Range(1, generanteChance) != 1) continue;
            ExitDirection direction = ExitDirectionExtensions.From(exit.name);
            Vector2Int attachedRoomGridPos = baseRoomGridPos + direction.ToVector();
            if (attachedRoomGridPos.x >= currentDungeon.GetLength(0) || attachedRoomGridPos.x < 0 || 
                attachedRoomGridPos.y >= currentDungeon.GetLength(1) || attachedRoomGridPos.y < 0) continue;
            if (currentDungeon[attachedRoomGridPos.x, attachedRoomGridPos.y] != null) continue;

            DungeonRoom attachedRoom = CreateRandomRoom(g => g.WallsObject.Find(direction.Opposite().ToString()) != null);
            if (attachedRoom == null) continue;

            AttachRoomTo(baseRoom, attachedRoom, direction);
            attachedRoom.RoomObject.name = attachedRoomGridPos.ToString();
            generatedRooms.Add(attachedRoom);
        }

        return generatedRooms.ToArray();
    }

    void AttachRoomTo(DungeonRoom baseRoom, DungeonRoom attacher, ExitDirection baseFrom)
    {
        Transform baseWalls = baseRoom.WallsObject;
        Transform attachWalls = attacher.WallsObject;
        Tilemap baseTM = baseWalls.GetComponent<Tilemap>();
        Tilemap attachTM = attachWalls.GetComponent<Tilemap>();

        Vector2Int baseGrid = GridPositionFromLocal(baseRoom.RoomObject.transform.localPosition);
        Vector2Int attacherGrid = baseGrid + baseFrom.ToVector();
        attacher.RoomObject.transform.localPosition = LocalPositionFromGrid(attacherGrid);
        currentDungeon[attacherGrid.x, attacherGrid.y] = attacher;

        baseTM.SetTile(Vector3Int.FloorToInt(baseWalls.transform.Find(baseFrom.ToString()).localPosition), null);
        Destroy(baseWalls.transform.Find(baseFrom.ToString()).gameObject);
        attachTM.SetTile(Vector3Int.FloorToInt(attachWalls.transform.Find(baseFrom.Opposite().ToString()).localPosition), null);
        Destroy(attachWalls.transform.Find(baseFrom.Opposite().ToString()).gameObject);
    }

    void Update()
    {

    }

    private class DungeonRoom
    {
        public DungeonRoom(Transform room) {
            RoomObject = room;
            WallsObject = room.Find("Walls");
            
            Tilemap map = WallsObject.GetComponent<Tilemap>();
            Bounds roomBounds = map.localBounds;

            Width = (int) Math.Ceiling(roomBounds.size.x/(double)ROOM_WIDTH);
            Height = (int) Math.Ceiling(roomBounds.size.y/(double)ROOM_HEIGHT);
        }

        public int Width { get; }
        public int Height { get; }
        public Transform RoomObject { get; }
        public Transform WallsObject {get; }

        public Transform[] GetExits(int x = 0, int y = 0)
        {
            List<Transform> exits = new List<Transform>();

            int lowX = x * ROOM_WIDTH;
            int lowY = y * ROOM_HEIGHT;
            int highX = (x + 1) * ROOM_WIDTH;
            int highY = (y + 1) * ROOM_HEIGHT;

            foreach (Transform child in WallsObject)
            {
                if (child.localPosition.x < lowX) continue;
                if (child.localPosition.x >= highX) continue;
                if (child.localPosition.y < lowY) continue;
                if (child.localPosition.y >= highY) continue;
                exits.Add(child);
            }

            return exits.ToArray();
        }
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