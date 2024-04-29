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

    // For towards player
    protected bool currentlyShooting = false;
    protected float currentShootingDelay = 0f;
    protected int currentShootCount = 0;
    // For spinning
    protected float currRotation = 0f;

    void Update()
    {
        currRotation += Time.deltaTime * shootBehaviour.spinSpeed;
        currRotation %= 360;

        if (currentlyShooting)
        {
            WhileShooting();
            return;
        }

        if (!shootManually) delay += Time.deltaTime;
        if (delay >= shootBehaviour.delay) HandleSpawn();
    }

    void HandleSpawn()
    {
        delay = 0f;
        switch (shootBehaviour.spawnBehaviour)
        {
            case SpawnerType.Arc:
                HandleArc();
                break;
            default:
                currentShootCount = shootBehaviour.shootCount;
                currentlyShooting = true;
                break;
        }
    }

    void HandleArc()
    {
        Vector2 dir = ((Vector2) transform.InverseTransformPoint(PlayerLiving.Instance.transform.position)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        for (int i = 0; i < shootBehaviour.shootCount; i++)
        {
            Projectile projectile = CreateProjectile(projectileBehaviour);
            projectile.transform.rotation = Quaternion.Euler(0, 0, angle - (shootBehaviour.gap * (shootBehaviour.shootCount/2)) + (shootBehaviour.gap * i) - 90);
        }
    }

    void WhileShooting()
    {
        currentShootingDelay += Time.deltaTime;
        if (currentShootingDelay < shootBehaviour.delayBetweenShoots) return;
        currentShootingDelay = 0f;
        currentShootCount--;

        if (shootBehaviour.spawnBehaviour == SpawnerType.TowardsPlayer) HandleTowardsPlayer();
        else HandleSpin();

        if (currentShootCount == 0) currentlyShooting = false;
    }

    void HandleTowardsPlayer()
    {
        Projectile projectile = CreateProjectile(projectileBehaviour);
        Vector2 dir = ((Vector2) transform.InverseTransformPoint(PlayerLiving.Instance.transform.position)).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle - 90);

        projectile.transform.rotation = rotation;
    }

    void HandleSpin()
    {
        Projectile projectile = CreateProjectile(projectileBehaviour);
        projectile.transform.rotation = Quaternion.Euler(0, 0, currRotation);
    }

    Projectile CreateProjectile(ProjectileBehaviour behaviour = null)
    {
        Projectile projectile = Instantiate(projectilePrefab.gameObject, transform).GetComponent<Projectile>();
        projectile.transform.localPosition = Vector3.zero;
        if (behaviour) projectile.behaviour = behaviour;
        return projectile;
    }

    public void ForceShoot()
    {
        delay = shootBehaviour.delay;
    }
}
