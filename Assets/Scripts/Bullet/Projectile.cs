using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileLife = 1f;
    public float projectileSpeed = 1f;

    private Rigidbody2D rb;
    private float timer = 0f;
    private Vector2 spawnPoint;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPoint = rb.position;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > projectileLife)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        Vector2 movement = rb.position + (Vector2)transform.right * projectileSpeed * Time.fixedDeltaTime;
        rb.MovePosition(movement);
    }

    public void UpdateMoveSpeed(float newProjectileSpeed)
    {
        projectileSpeed = newProjectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        PlayerLiving player = collision.gameObject.GetComponent<PlayerLiving>();
        if (player != null)
        {
            player.Damage(20);
        }
    }

    public void SetProjectileAttributes(float speed, float life)
    {
        projectileSpeed = speed;
        projectileLife = life;
    }
    
}
