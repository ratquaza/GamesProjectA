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
    [SerializeField] private int maxRooms = 5;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    private Room[] roomPool;

    private Grid grid;
    private Room[,] floor;
    
    void Start()
    {
        grid = GetComponent<Grid>();

        // Take the supplied room GameObjects, turn them into Rooms
        roomPool = suppliedRoomPool.Select(room => new Room(room.transform)).ToArray();
        // Create the 2D array thats the current dungeon floor
        floor = new Room[10, 10];

        int x = 0;
        int y = x;

        Room spawnRoom = roomPool[0].Clone(grid.transform);
        Place(spawnRoom, new Vector2Int(x, y));

        List<Room> queue = new List<Room>(FillExits(spawnRoom, 1));
        while (maxRooms > 0) {
            int count = queue.Count;
            for (int i = 0; i < count; i++)
            {
                queue.AddRange(FillExits(queue[i], 1, 3));
            }
            queue.RemoveRange(0, count);
            if (queue.Count == 0) break;
            maxRooms -= count;
        }

        player.transform.localPosition = new Vector3(ROOM_WIDTH/2, ROOM_HEIGHT/2, 1);
    }

    // Convert a Vector2Int coordinate position for the floor into a Vector3 position 
    // for Transform#localPosition 
    public Vector3 FloorToLocal(Vector2Int pos)
    {
        return new Vector3(pos.x * ROOM_WIDTH, pos.y * ROOM_HEIGHT, 0);
    }

    // Convert a 3D Vector3 position into a Vector2Int coordinate for the floor grid 
    public Vector2Int LocalToFloor(Vector3 position)
    {
        return new Vector2Int((int) position.x/ROOM_WIDTH, (int) position.y/ROOM_HEIGHT);
    }

    // Get all Rooms that fit the criteria
    private Room[] FilterRooms(Predicate<Room> predicate)
    {
        return roomPool.Where(predicate.Invoke).ToArray();
    }

    // Attempt to generate Rooms on every exit for the supplied Room
    // generateChance is a 1/x chance that a Room is successfully made
    // conjoinChance is a 1/x chance that when an exit is adjacent to another existing Room, it joins them together 
    private Room[] FillExits(Room baseRoom, int generanteChance = 1, int conjoinChance = 3)
    {
        generanteChance = Math.Max(1, generanteChance);
        conjoinChance = Math.Max(1, conjoinChance);

        Vector2Int basePos = LocalToFloor(baseRoom.transform.localPosition);
        List<Room> generatedRooms = new List<Room>();

        foreach (Transform baseExit in baseRoom.wallTransform)
        {
            if (generanteChance > 1 && UnityEngine.Random.Range(1, generanteChance) != 1) continue;
            
            Vector2Int baseExitQuadrant = baseRoom.GetExitsQuadrant(baseExit);
            ExitDirection baseExitDirection = ExitDirections.From(baseExit.name);
            Vector2Int targetLocation = basePos + baseExitQuadrant + baseExitDirection.Vector();
            if (!InBounds(targetLocation)) continue;

            if (floor[targetLocation.x, targetLocation.y] != null)
            {
                if (conjoinChance > 1 && UnityEngine.Random.Range(1, conjoinChance) != 1) continue;
                Room existingRoom = floor[targetLocation.x, targetLocation.y];
                Vector2Int quadrantHit = existingRoom.gridPosition - targetLocation;
                Transform possibleExit = existingRoom.GetExitAt(quadrantHit, baseExitDirection.Opposite());

                if (possibleExit == null) continue;
                baseRoom.OpenExit(baseExit);
                existingRoom.OpenExit(possibleExit);
                continue;
            }

            List<Jigsaw> pieces = new List<Jigsaw>();
            foreach (Room possibleRoom in FilterRooms(g => g.wallTransform.Find(baseExitDirection.Opposite().ToString()) != null))
            {
                Transform[] possibleExits = possibleRoom.GetAllExits(baseExitDirection.Opposite());
                foreach (Transform pExit in possibleExits)
                {
                    Vector2Int quad = possibleRoom.GetExitsQuadrant(pExit);
                    if (Fits(possibleRoom, targetLocation, quad)) pieces.Add(
                        new Jigsaw(possibleRoom, baseExitDirection.Opposite(), quad)
                    );
                }
            }
            
            if (pieces.Count == 0) continue;
            Jigsaw selectedPiece = pieces[UnityEngine.Random.Range(0, pieces.Count)];
            Room targetRoom = selectedPiece.room.Clone(grid.transform);

            if (enemy != null && UnityEngine.Random.Range(0, 3) == 0)
            {
                for (int x = 0; x < targetRoom.width; x++)
                {
                    for (int y = 0; y < targetRoom.height; y++)
                    {
                        GameObject enemyClone = Instantiate(enemy, targetRoom.transform);
                        enemyClone.transform.localPosition = new Vector3(ROOM_WIDTH/2 + x * ROOM_WIDTH, ROOM_HEIGHT/2 + y * ROOM_HEIGHT, 3);
                    }
                }
            }

            Place(targetRoom, targetLocation, selectedPiece.quad);
            baseRoom.OpenExit(baseExit);
            targetRoom.OpenExit(targetRoom.GetExitAt(selectedPiece.quad, selectedPiece.direction));
            generatedRooms.Add(targetRoom);
        }

        return generatedRooms.ToArray();
    }

    // Whether the coordinate is within bounds
    private bool InBounds(Vector2Int at)
    {
        return InBounds(at, Vector2Int.zero);
    }

    // Whether the coordinate and a box extending from it is within bounds
    private bool InBounds(Vector2Int at, Vector2Int size)
    {
        return at.x >= 0 && at.x + size.x < floor.GetLength(0) && at.y >= 0 && at.y + size.y < floor.GetLength(1);
    }

    // Whether the Room fits at a coordinate, checking whether its within bounds
    // and not overlapping other rooms
    private bool Fits(Room room, Vector2Int at)
    {
        if (!InBounds(at, room.area)) return false;
        for (int x = at.x; x < at.x + room.width; x++) {
            for (int y = at.y; y < at.y + room.height; y++) {
                if (floor[x, y] != null) return false;
            }
        }

        return true;
    }

    // Whether the Room fits at a coordinate, checking whether its within bounds
    // and not overlapping other rooms, using a specific quadrant of the Room
    private bool Fits(Room room, Vector2Int at, Vector2Int quadrant)
    {
        return Fits(room, at - quadrant);
    }


    // Places a Room at the given coordinate, updating the floor grid
    // NOTE: Does not perform checks, call Fits() before calling this to prevent errors
    private void Place(Room room, Vector2Int position)
    {
        Vector3 localSpace = FloorToLocal(position);
        room.transform.localPosition = localSpace;
        room.gridPosition = position;
        room.transform.name = position.ToString();
        for (int x = position.x; x < position.x + room.width; x++)
        {
            for (int y = position.y; y < position.y + room.height; y++)
            {
                floor[x, y] = room;
            }
        }
    }

    // Places a Room at the given coordinate, updating the floor grid, using a 
    // specific quadrant of the Room
    private void Place(Room room, Vector2Int position, Vector2Int quadrant)
    {
        Place(room, position - quadrant);
    }

    // Room class, provides helper functions
    private class Room
    {
        public Room(Transform room) {
            this.transform = room;
            this.wallTransform = room.Find("Walls");
            
            // Auto-compress bounds
            Tilemap map = wallTransform.GetComponent<Tilemap>();
            map.CompressBounds();
            BoundsInt roomBounds = map.cellBounds;

            width = (int) Math.Ceiling(roomBounds.size.x/(double)ROOM_WIDTH);
            height = (int) Math.Ceiling(roomBounds.size.y/(double)ROOM_HEIGHT);
        }

        // Width and height of the room by floor grid coords, not tiles
        public int width { get; }
        public int height { get; }
        // The transform of the root GameObject
        public Transform transform { get; }
        // The transform of the wall GameObject, holds all exits and colliders
        public Transform wallTransform { get; }
        // Area of the room by floor grid coords
        public Vector2Int area { get { return new Vector2Int(width, height); } }
        // Position of the room in the grid
        public Vector2Int gridPosition { get; set; } = new Vector2Int(-1, -1);

        // Gets the specific exit by the quadrant of the room and the direction
        public Transform GetExitAt(Vector2Int quadrant, ExitDirection direction)
        {
            foreach (Transform exit in GetAllExits(direction))
            {
                if (GetExitsQuadrant(exit) == quadrant) return exit;   
            }
            return null;
        }

        // Gets all exits by a direction
        public Transform[] GetAllExits(ExitDirection direction)
        {
            List<Transform> exits = new List<Transform>();
            foreach (Transform exit in wallTransform)
            {
                if (exit.name == direction.ToString()) exits.Add(exit);
            }
            return exits.ToArray();
        }

        // Gets the quadrant that an exit is in
        public Vector2Int GetExitsQuadrant(Transform exit)
        {
            return new Vector2Int(
                (int) Math.Floor(exit.transform.localPosition.x/(double)ROOM_WIDTH),
                (int) Math.Floor(exit.transform.localPosition.y/(double)ROOM_HEIGHT)
            );
        }

        // 'Opens' an exit by removing the wall tile
        public void OpenExit(Transform exit)
        {
            if (exit.parent != wallTransform) return;
            wallTransform.GetComponent<Tilemap>().SetTile(Vector3Int.FloorToInt(exit.transform.localPosition), null);
        }

        // Copies the room
        public Room Clone(Transform parent)
        {
            return new Room(Instantiate(transform, parent));
        }
    }
    
    private struct Jigsaw
    {
        public Room room;
        public ExitDirection direction;
        public Vector2Int quad;

        public Jigsaw(Room room, ExitDirection direction, Vector2Int quad)
        {
            this.room = room;
            this.direction = direction;
            this.quad = quad;
        }
    }
}

// Cardinal directions enum
public enum ExitDirection
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

// Helper functions for exit directions
public static class ExitDirections
{
    // ExitDirection to Vector2Int
    private static readonly Vector2Int[] vectors = new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    // Parse String to ENum
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

    // Enum to Vector2Int
    public static Vector2Int Vector(this ExitDirection exit)
    {
        return vectors[(int) exit];
    }

    // Flips the Enum
    public static ExitDirection Opposite(this ExitDirection exit)
    {
        return (ExitDirection) (((int) exit + 2) % 4);
    }
}