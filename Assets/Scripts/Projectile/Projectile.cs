using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public ProjectileBehaviour behaviour;
    private float lifetime = 0f;

    public Vector2 forwardsDirection;

    void Start()
    {
        transform.localScale = Vector3.one * behaviour.size;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning("Projectile doesn't have SpriteRenderer!");
            return;
        }
        spriteRenderer.sprite = behaviour.projectileSprite;
    }

    void Update()
    {
        transform.position += (Vector3) GetMovement() * behaviour.speed * Time.deltaTime;
        lifetime += Time.deltaTime;
        if (lifetime >= behaviour.lifetime) Destroy(gameObject);
    }

    Vector2 GetMovement()
    {
        switch (behaviour.moveBehaviour)
        {
            case MoveBehaviour.Homing:
                return ((Vector2) (PlayerLiving.Instance.transform.position - transform.position)).normalized;
            default:
                return forwardsDirection;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLiving player = collider.GetComponent<PlayerLiving>();
        if (player == null)
        {
            if (behaviour.destroyOnWall)
            {
                Destroy(gameObject);
            }
            else if (behaviour.ricochetOnWall)
            {
                Vector2 closestPoint = collider.ClosestPoint((Vector2) transform.position + GetMovement() * .1f);
                Vector2 normal = ((Vector2) transform.position - closestPoint).normalized;
                forwardsDirection = Vector2.Reflect(forwardsDirection, normal);
            }
            return;
        }
        player.TakeDamage((int) Math.Round(behaviour.damage));
        Destroy(gameObject);
    }
}