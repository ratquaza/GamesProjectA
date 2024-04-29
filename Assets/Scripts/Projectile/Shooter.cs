using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] public ShootBehaviour shootBehaviour;
    [SerializeField] public ProjectileBehaviour projectileBehaviour;
    [SerializeField] public Projectile projectilePrefab;
    [SerializeField] public bool shootManually = false;

    protected float delay = 0f;
    protected bool currentlyShooting = false;
    protected float currentShootingDelay = 0f;
    protected int currentShootCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (currentlyShooting)
        {
            currentShootingDelay += Time.deltaTime;
            if (currentShootingDelay < shootBehaviour.delayBetweenShoots) return;
            currentShootingDelay = 0f;
            currentShootCount--;

            Projectile projectile = Instantiate(projectilePrefab.gameObject, transform).GetComponent<Projectile>();
            Vector2 startPos = shootBehaviour.spawnBehaviour == SpawnerType.Forwards ? 
                transform.right :
                ((Vector2) (PlayerLiving.Instance.transform.position - transform.position)).normalized;

            projectile.transform.localPosition = startPos;
            projectile.behaviour = projectileBehaviour;
            projectile.transform.rotation = transform.rotation; 

            if (currentShootCount == 0) currentlyShooting = false;
            return;
        }

        delay += Time.deltaTime;
        if (delay < shootBehaviour.delay) return;
        delay = 0f;

        switch (shootBehaviour.spawnBehaviour)
        {
            case SpawnerType.Forwards:
            case SpawnerType.TowardsPlayer:
                currentShootCount = shootBehaviour.shootCount;
                currentlyShooting = true;
                break;
            case SpawnerType.Spinning:

                break;
            case SpawnerType.Arc:

                break;
        }
    }
}
