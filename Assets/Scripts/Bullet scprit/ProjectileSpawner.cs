using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    enum SpawnerType
    {
        Straight, 
        Spin
    }

    [Header("Projectile Attributes")]
    public GameObject projectile;
    [SerializeField] private float projectileLife = 10f;
    [SerializeField] private float projectileSpeed = 5f;

    [Header("Spawner Attributes")] 
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;

    private GameObject spawnedProjectile;
    private float timer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(spawnerType == SpawnerType.Spin)
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z+1f);
        }
        if(timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void Fire()
    {
        if(projectile)
        {
            spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            spawnedProjectile.GetComponent<Projectile>().projectileSpeed = projectileSpeed;
            spawnedProjectile.GetComponent<Projectile>().projectileLife =projectileLife;
            spawnedProjectile.transform.rotation = transform.rotation;

        }
    }
}
 