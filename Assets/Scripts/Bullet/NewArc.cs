using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewArc : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private float shootTime = 2f;
    [SerializeField] private float degree = 10f;
    [SerializeField] private int shootCount = 7;

    private float curShootTime = 0f;

    // Update is called once per frame
    void Update()
    {
        curShootTime += Time.deltaTime;
        if (curShootTime > shootTime)
        {
            Shoot();
            curShootTime = 0;
        }
    }

    void Shoot()
    {
        Vector2 toPlayer = ((Vector2) (player.transform.position - transform.position)).normalized;
        float angle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < shootCount; i++)
        {
            GameObject bullet = Instantiate(projectile);
            bullet.transform.rotation = Quaternion.Euler(Vector3.forward * (angle - (degree * (shootCount/2)) + (degree * i)));
            bullet.transform.position = transform.position;
            Projectile proj = bullet.GetComponent<Projectile>();
            proj.projectileSpeed = 5f;
            proj.projectileLife = 10f;
        }
    }
}
