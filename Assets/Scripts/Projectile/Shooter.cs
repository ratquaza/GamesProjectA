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

        float angleToPlayer = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float gap = shootBehaviour.arcDegrees/(shootBehaviour.shootCount - 1);

        for (int i = 0; i < shootBehaviour.shootCount; i++)
        {
            Projectile projectile = CreateProjectile(projectileBehaviour);
            projectile.forwardsDirection = Quaternion.Euler(0, 0, angleToPlayer - shootBehaviour.arcDegrees/2f + gap * i) * Vector2.right;
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
        projectile.forwardsDirection = dir;
    }

    void HandleSpin()
    {
        Projectile projectile = CreateProjectile(projectileBehaviour);
        if (shootBehaviour.spawnBehaviour == SpawnerType.Spinning)
        {
            projectile.forwardsDirection = Quaternion.Euler(0, 0, currRotation) * Vector2.up;
        }
        else
        {
            Vector2 dir = ((Vector2) transform.InverseTransformPoint(PlayerLiving.Instance.transform.position)).normalized;
            projectile.forwardsDirection = dir;
            projectile.transform.localPosition = Quaternion.Euler(0, 0, currRotation) * Vector2.up * .15f;
        }
    }

Projectile CreateProjectile(ProjectileBehaviour behaviour = null)
    {
        Projectile projectile = Instantiate(projectilePrefab.gameObject, null).GetComponent<Projectile>();

        
        projectile.transform.localPosition = this.GetComponentInParent<Transform>().position;
        if (behaviour) projectile.behaviour = behaviour;
        return projectile;
    }

    public void ForceShoot()
    {
        delay = shootBehaviour.delay;
    }
}
