using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcProjectileSpawner : MonoBehaviour
{
    public GameObject projectile;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float firingRate = 2f;
    [SerializeField] private PlayerLiving player;

    private float currentFiringRate = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentFiringRate += Time.deltaTime;
        if(currentFiringRate >= firingRate)
        {
            Fire();
            currentFiringRate = 0;
        }
    }

    private void Fire()
    {

        Vector2 toPlayer = ((Vector2) (player.transform.position - transform.position)).normalized;
        toPlayer = Quaternion.AngleAxis(-45f, Vector3.forward) * toPlayer;

        for (int i = 0; i < 7; i++)
        {
            GameObject newProj = Instantiate(projectile, transform);
            newProj.transform.rotation = Quaternion.Euler(toPlayer);
            newProj.transform.localPosition = Vector3.zero;
            Debug.Log(Quaternion.Euler(toPlayer));
            newProj.GetComponent<Projectile>().projectileSpeed = 3f;
            newProj.GetComponent<Projectile>().projectileLife = 5f;
            toPlayer = Quaternion.AngleAxis(15f, Vector3.forward) * toPlayer;
        }
    }
}
 