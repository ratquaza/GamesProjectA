using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public GameObject player; 
    public GameObject shooter1; 
    public GameObject shooter2; 
    public GameObject shooter3; 

    private float bossChange;
    public float distanceThreshold1 = 10f; 
    public float distanceThreshold2 = 20f; 
    public Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        bossChange = enemy.Health();
        bossChange /= 2;
    }



    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        shooter1.SetActive(false);
        shooter2.SetActive(false);
        shooter3.SetActive(false);

        if(enemy.Health() < bossChange){
            shooter1.SetActive(true);
        }

        if (distanceToPlayer <= distanceThreshold1)
        {
            shooter2.SetActive(true);
        }
        else if (distanceToPlayer >= distanceThreshold2)
        {
            shooter3.SetActive(true);
        }
    }
}