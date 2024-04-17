using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private ProjectileInfo projectileBehaviours;
    [SerializeField] private ProjectileShootInfo shootStyle;

    enum SpawnerType
    {
        Straight,
        Spin,
        NewArc
    }

    [Header("Projectile Attributes")]
    public GameObject projectilePrefab;
    [SerializeField] private float projectileLife = 10f;
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private int burstCount = 3;
    private float angle = 1 ;

    [Header("Spawner Attributes")]
    [SerializeField] private SpawnerType spawnerType;
    [SerializeField] private float firingRate = 1f;
    [SerializeField] private float firingDistance = 10f;

    [Header("NewArc")]
    [SerializeField] private float degree = 10f;
    [SerializeField] private int shootCount = 7;

    private float timer = 0f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
     float distance = Vector2.Distance(transform.position, player.transform.position);
            
        if (distance < firingDistance)
        {
            timer += Time.deltaTime;
            
            switch (shootStyle.shootStyle)
            {
                case ProjectileShootInfo.ShootStyle.Straight:
                    UpdateStraightSpawner();
                    break;
                case ProjectileShootInfo.ShootStyle.Spin:
                    UpdateSpinSpawner();
                    break;
            }

            if (timer >= firingRate)
            {
            
            for (int i = 0; i < burstCount; i++)
            {    
                Fire();
            }
            
            timer = 0;
            
            }
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

    public void UpdateNewArc()
    {

    }

    private void Fire()
    {
        if (spawnerType != SpawnerType.Spin)
        {
            Vector2 toPlayer = ((Vector2) (player.transform.position - transform.position)).normalized;
            angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;  
        }
        if (projectilePrefab)
        {
            GameObject spawnedProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            
            if (spawnerType != SpawnerType.Spin)
            {
                spawnedProjectile.transform.rotation = Quaternion.Euler(Vector3.forward * angle);
                spawnedProjectile.transform.position = transform.position;
            }
            Projectile projectile = spawnedProjectile.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetProjectileAttributes(projectileSpeed, projectileLife);
            }
        }

       }
    
    
}
