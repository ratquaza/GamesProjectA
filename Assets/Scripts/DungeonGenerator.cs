using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] rooms;
    [SerializeField] private int count = 5;
    private readonly string[] exitNames = new string[4] { "NorthExit", "EastExit", "SouthExit", "WestExit" };
    private Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        grid = GetComponent<Grid>();

        GameObject chosenRoom = Instantiate(RandomRoom(), grid.transform);
        Tilemap chosenTm = chosenRoom.GetComponent<Tilemap>();
        chosenTm.CompressBounds();
        Bounds chosenBounds = chosenTm.localBounds;

        chosenRoom.transform.localPosition = Vector3.zero - chosenBounds.center + chosenBounds.size / 2;
        chosenRoom.name = "SpawnRoom";
        int i = 100;

        do
        {
            foreach (GameObject r in FillExits(chosenRoom))
            {

            }
            i--;
        }
        while (i > 0);
    }

    GameObject[] FillExits(GameObject room, int generateChance = 1, int depth = 3)
    {
        List<GameObject> generatedRooms = new List<GameObject>();
        generateChance = Math.Min(generateChance, 1);

        for (int i = 0; i < room.transform.childCount; i++)
        {
            Transform child = room.transform.GetChild(i);
            if (!exitNames.Contains(child.name)) continue;
            string opposingExit = exitNames[(Array.IndexOf(exitNames, child.name) + 2) % 4];

            GameObject[] connectingRooms = rooms.Where((go) => go.transform.Find(opposingExit) != null).ToArray();
            if (connectingRooms.Length == 0) continue;
            GameObject chosenRoom = Instantiate(connectingRooms[UnityEngine.Random.Range(0, connectingRooms.Length)], grid.transform);
            chosenRoom.GetComponent<Tilemap>().CompressBounds();

            generatedRooms.Add(chosenRoom);
            GameObject chosenRoomExit = chosenRoom.transform.Find(opposingExit).gameObject; 
            AlignByAnchors(chosenRoom, chosenRoomExit, child.gameObject);
            DestroyImmediate(child.gameObject);
            DestroyImmediate(chosenRoomExit);
        }

        return generatedRooms.ToArray();
    }

    void AlignByAnchors(GameObject target, GameObject targetAnchor, GameObject otherAnchor)
    {
        Bounds targetBounds = target.GetComponent<Tilemap>().localBounds;
        Vector3 targetAnchorOffset = targetBounds.center - targetAnchor.transform.localPosition;

        target.transform.localPosition = otherAnchor.transform.position - targetBounds.center + targetAnchorOffset;
    }

    GameObject RandomRoom()
    {
        return rooms[UnityEngine.Random.Range(0, rooms.Length)];
    }

    // Update is called once per frame
    void Update()
    {

    }
}
