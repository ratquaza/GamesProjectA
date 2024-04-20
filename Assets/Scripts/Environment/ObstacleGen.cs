using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGen : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab1;
    [SerializeField] private GameObject obstaclePrefab2;
    [SerializeField] private GameObject obstaclePrefab3;

    void Start()
    {
        int randomInt = Random.Range(1, 100); // Generate a random number between 1 and 99

        // Determine which third the random number falls into
        if (randomInt >= 1 && randomInt <= 33)
        {
            Instantiate(obstaclePrefab1, transform.position, Quaternion.identity);
        }
        else if (randomInt >= 34 && randomInt <= 66)
        {
            Instantiate(obstaclePrefab2, transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(obstaclePrefab3, transform.position, Quaternion.identity);
        }
    }
}
