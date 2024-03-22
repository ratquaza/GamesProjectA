using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private GameObject testRoom;

    // Start is called before the first frame update
    void Start()
    {
        Grid grid = GetComponent<Grid>();
        int count = 2;

        GameObject lastRoom = null;

        while (count > 0)
        {
            GameObject chosenRoom = Instantiate(testRoom, grid.transform);
            Tilemap chosenTm = chosenRoom.GetComponent<Tilemap>();
            chosenTm.CompressBounds();
            Bounds chosenBounds = chosenTm.localBounds;

            if (lastRoom != null)
            {
                GameObject lastExit = lastRoom.transform.Find("WestExit").gameObject;
                GameObject chosenExit = chosenRoom.transform.Find("EastExit").gameObject;
                AlignByAnchors(chosenRoom, chosenExit, lastExit);
            } else 
            {
                chosenRoom.transform.localPosition = Vector3.zero - chosenBounds.center;
            }

            lastRoom = chosenRoom;
            count--;
        }

    }

    void AlignByAnchors(GameObject target, GameObject targetAnchor, GameObject otherAnchor)
    {
        Bounds targetBounds = target.GetComponent<Tilemap>().localBounds;
        Vector3 targetAnchorOffset = targetBounds.center - targetAnchor.transform.localPosition;

        target.transform.localPosition = otherAnchor.transform.position - targetBounds.center + targetAnchorOffset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
