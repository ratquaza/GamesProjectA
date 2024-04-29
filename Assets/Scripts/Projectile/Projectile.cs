using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public ProjectileBehaviour behaviour;
    private float lifetime = 0f;

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
        Vector2 moveDir = Vector2.zero;

        switch (behaviour.moveBehaviour)
        {
            case MoveBehaviour.Forward:
                moveDir = transform.up;
                break;
            case MoveBehaviour.Homing:
                moveDir = ((Vector2) (PlayerLiving.Instance.transform.position - transform.position)).normalized;
                break;
        }

        transform.position += (Vector3) moveDir * behaviour.speed * Time.deltaTime;
        lifetime += Time.deltaTime;
        if (lifetime >= behaviour.lifetime) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLiving player = collider.GetComponent<PlayerLiving>();
        if (player == null)
        {
            if (behaviour.destroyOnWall) Destroy(gameObject);
            return;
        }
        player.Damage((int) Math.Round(behaviour.damage));
        Destroy(gameObject);
    }
}