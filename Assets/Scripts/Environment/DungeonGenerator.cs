using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    public static readonly int ROOM_WIDTH = 15;
    public static readonly int ROOM_HEIGHT = 11;

    [SerializeField] private DungeonRoom[] roomPool;
    [SerializeField] private int maxRooms = 5;
    [SerializeField] private GameObject enemy;
    [SerializeField] private CameraFollow cameraScript;

    private Grid grid;
    private DungeonRoom[,] floor;
    
    void Start()
    {
        grid = GetComponent<Grid>();

        // Create the 2D array thats the current dungeon floor
        floor = new DungeonRoom[15, 15];

        int x = 8;
        int y = x;

        DungeonRoom spawnRoom =  Instantiate(roomPool[0], grid.transform);
        Place(spawnRoom, new Vector2Int(x, y));

        List<DungeonRoom> rooms = new List<DungeonRoom>(FillExits(spawnRoom, 1, 1));
        while (maxRooms > 0)
        {
            if (rooms.Count == 0) break;
            DungeonRoom room = rooms[0];
            rooms.RemoveAt(0);
            DungeonRoom[] genRooms = FillExits(room, 3, 3);
            rooms.AddRange(genRooms);
            maxRooms -= genRooms.Length;
        }

        PlayerLiving.Instance.transform.localPosition = new Vector2(x * ROOM_WIDTH + ROOM_WIDTH/2, y * ROOM_HEIGHT + ROOM_HEIGHT/2);
    }

    // Convert a Vector2Int coordinate position for the floor into a Vector3 position 
    // for Transform#localPosition 
    public Vector3 GridToLocal(Vector2Int pos)
    {
        return new Vector3(pos.x * ROOM_WIDTH, pos.y * ROOM_HEIGHT, 0);
    }

    // Convert a 3D Vector3 position into a Vector2Int coordinate for the floor grid 
    public Vector2Int LocalToGrid(Vector3 position)
    {
        return new Vector2Int((int) position.x/ROOM_WIDTH, (int) position.y/ROOM_HEIGHT);
    }

    // Get all Rooms that fit the criteria
    private DungeonRoom[] FilterRooms(Predicate<DungeonRoom> predicate)
    {
        return roomPool.Where(predicate.Invoke).ToArray();
    }

    // Attempt to generate Rooms on every exit for the supplied DungeonRoom
    // generateChance is a 1/x chance that a DungeonRoom is successfully made
    // conjoinChance is a 1/x chance that when an exit is adjacent to another existing DungeonRoom, it joins them together 
    private DungeonRoom[] FillExits(DungeonRoom room, int generanteChance = 1, int conjoinChance = 3)
    {
        generanteChance = Math.Max(1, generanteChance);
        conjoinChance = Math.Max(1, conjoinChance);

        Vector2Int origin = LocalToGrid(room.transform.localPosition);
        List<DungeonRoom> generatedRooms = new List<DungeonRoom>();

        foreach (Quadrant border in room.borders)
        {
            foreach (ExitDirection exit in border.Directions())
            {
                if (generanteChance > 1 && UnityEngine.Random.Range(1, generanteChance) != 1) continue;
                
                Vector2Int targetLocation = origin + border.position + exit.Vector();
                if (!InBounds(targetLocation)) continue;

                if (floor[targetLocation.x, targetLocation.y] != null)
                {
                    if (conjoinChance > 1 && UnityEngine.Random.Range(1, conjoinChance) != 1) continue;
                    ConjoinRooms(room, border, exit);
                    continue;
                }
                
                DungeonRoom targetRoom = AttachRandomRoom(room, border, exit);
                if (targetRoom) generatedRooms.Add(targetRoom);
            }
        }

        return generatedRooms.ToArray();
    }

    // Attach a random room to the supplied room at the given quadrant and exit
    private DungeonRoom AttachRandomRoom(DungeonRoom room, Quadrant quad, ExitDirection exit)
    {
        Vector2Int targetLocation = room.gridPosition + quad.position + exit.Vector();
        Jigsaw[] pieces = FindJigsaws(targetLocation, exit.Opposite());
        if (pieces.Length == 0) return null;
        Jigsaw selectedPiece = pieces[UnityEngine.Random.Range(0, pieces.Length)];
        DungeonRoom targetRoom = Instantiate(selectedPiece.room.gameObject, grid.transform).GetComponent<DungeonRoom>();

        Place(targetRoom, targetLocation, selectedPiece.quad.position);
        room.ClearExit(quad, exit);
        targetRoom.ClearExit(selectedPiece.quad, exit.Opposite());
        return targetRoom;
    }

    // Generalte all valid jigsaw pieces at the given position with the given exit direction
    private Jigsaw[] FindJigsaws(Vector2Int position, ExitDirection exit)
    {
        List<Jigsaw> pieces = new List<Jigsaw>();
        foreach (DungeonRoom possibleRoom in FilterRooms(g => g.GetWithExit(exit).Count() != 0))
        {
            Quadrant[] validQuads = possibleRoom.GetWithExit(exit);
            foreach (Quadrant quadExit in validQuads)
            {
                if (Fits(possibleRoom, position, quadExit.position)) pieces.Add(new Jigsaw(possibleRoom, exit, quadExit));
            }
        }
        return pieces.ToArray();
    }

    // Finds the room oppositng the supplied room at it's given quadrant for its given exit and opens the exits
    private void ConjoinRooms(DungeonRoom room, Quadrant quad, ExitDirection exit)
    {
        Vector2Int targetLocation = room.gridPosition + quad.position + exit.Vector();
        DungeonRoom existingRoom = floor[targetLocation.x, targetLocation.y];
        if (existingRoom == null) return;
        Quadrant? possibleExit = existingRoom.GetQuadrant(existingRoom.gridPosition - targetLocation);
        if (possibleExit == null || !possibleExit.Value.CanExit(exit.Opposite())) return;
        room.ClearExit(quad, exit);
        existingRoom.ClearExit(possibleExit.Value, exit.Opposite());
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

    // Whether the DungeonRoom fits at a coordinate, checking whether its within bounds
    // and not overlapping other rooms
    private bool Fits(DungeonRoom room, Vector2Int at)
    {
        if (!InBounds(at, room.area)) return false;
        for (int x = at.x; x < at.x + room.width; x++) {
            for (int y = at.y; y < at.y + room.height; y++) {
                if (floor[x, y] != null) return false;
            }
        }
        return true;
    }

    // Whether the DungeonRoom fits at a coordinate, checking whether its within bounds
    // and not overlapping other rooms, using a specific quadrant of the DungeonRoom
    private bool Fits(DungeonRoom room, Vector2Int at, Vector2Int quadrant)
    {
        return Fits(room, at - quadrant);
    }

    // Places a DungeonRoom at the given coordinate, updating the floor grid
    // NOTE: Does not perform checks, call Fits() before calling this to prevent errors
    private void Place(DungeonRoom room, Vector2Int position)
    {
        Vector3 localSpace = GridToLocal(position);
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

    // Places a DungeonRoom at the given coordinate, updating the floor grid, using a 
    // specific quadrant of the DungeonRoom
    private void Place(DungeonRoom room, Vector2Int position, Vector2Int quadrant)
    {
        Place(room, position - quadrant);
    }
    private struct Jigsaw
    {
        public DungeonRoom room;
        public ExitDirection direction;
        public Quadrant quad;

        public Jigsaw(DungeonRoom room, ExitDirection direction, Quadrant quad)
        {
            this.room = room;
            this.direction = direction;
            this.quad = quad;
        }
    }
}

[Serializable]
public struct Quadrant
{
    public Vector2Int position;
    public bool northExit;
    public bool eastExit;
    public bool southExit;
    public bool westExit;

    public ExitDirection[] Directions()
    {
        List<ExitDirection> directions = new List<ExitDirection>();
        if (northExit) directions.Add(ExitDirection.North);
        if (eastExit) directions.Add(ExitDirection.East);
        if (southExit) directions.Add(ExitDirection.South);
        if (westExit) directions.Add(ExitDirection.West);
        return directions.ToArray();
    }

    public bool CanExit(ExitDirection exit)
    {
        switch (exit)
        {
            case ExitDirection.North: return northExit;
            case ExitDirection.East: return eastExit;
            case ExitDirection.South: return southExit;
            default: return westExit;
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