using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    enum SpawnerType
    {
        Straight,
        Spin
    }

    [Header("Projectile Attributes")]
    public GameObject projectilePrefab;
    [SerializeField] private float projectileLife = 10f;
    [SerializeField] private float projectileSpeed = 5f;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        switch (spawnerType)
        {
            case SpawnerType.Straight:
                UpdateStraightSpawner();
                break;
            case SpawnerType.Spin:
                UpdateSpinSpawner();
                break;
        }

        if (timer >= firingRate)
        {
            Fire();
            timer = 0;
        }
    }

    private void UpdateStraightSpawner()
    {
        /* Add logic if needed uwu */
    }

    private void UpdateSpinSpawner()
    {
        transform.Rotate(Vector3.forward, Time.deltaTime * 360f);
    }


    private void Fire()
    {
        if (projectilePrefab)
        {
            GameObject spawnedProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            ProjectileOld projectile = spawnedProjectile.GetComponent<ProjectileOld>();
            if (projectile != null)
            {
                projectile.SetProjectileAttributes(projectileSpeed, projectileLife);
            }
        }
    }

    
}
