using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public abstract void OnEquip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput);
    public abstract void OnUnequip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput);

    // Simple helper function that handles finding all enemies in a trigger collider, damaging and knocking them
    public static void DamageInCollider(Collider2D collider, float baseDamage, Vector2 kb)
    {
        DamageInCollider(collider, baseDamage, (enemy) => kb);
    }

    // Simple helper function that handles finding all enemies in a trigger collider, damaging and calculating kb for each enemy
    public static void DamageInCollider(Collider2D collider, float baseDamage, Func<Enemy, Vector2> kb)
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D { layerMask = LayerMask.GetMask("Enemy", "EnemyProjectile"), useLayerMask = true };
        collider.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();

            if(enemy != null){
                enemy.TakeDamage((int) Math.Round(baseDamage));
                rb2d.velocity += kb.Invoke(enemy);
            }

            if (hit.gameObject.layer == LayerMask.NameToLayer("EnemyProjectile"))
            {
                Destroy(hit.gameObject);
            }

        }
    }

    public static void DamageInSquare(Vector2 pos, Vector2 bounds, float baseDamage, Func<Enemy, Vector2> kb)
    {
        List<RaycastHit2D> hits = new List<RaycastHit2D>();
        ContactFilter2D filter = new ContactFilter2D { layerMask = LayerMask.GetMask("Enemy"), useLayerMask = true };
        Physics2D.BoxCast(pos, bounds, 0, Vector2.zero, filter, hits);
        foreach (var hit in hits)
        {
            GameObject obj = hit.collider.gameObject;
            Enemy enemy = obj.GetComponent<Enemy>();
            Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();
            enemy.TakeDamage((int) Math.Round(baseDamage));
            rb2d.velocity += kb.Invoke(enemy);
        }
    }

    public static void DamageInSquare(Vector2 pos, Vector2 bounds, float baseDamage, Vector2 kb)
    {
        DamageInSquare(pos, bounds, baseDamage, (enemy) => kb);
    }
}
