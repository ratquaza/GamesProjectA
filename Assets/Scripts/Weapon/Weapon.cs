using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public abstract void OnEquip(PlayerLiving player, WeaponItem item, InputAction primaryInput, InputAction secondaryInput);
    public abstract void OnUnequip(PlayerLiving player, WeaponItem item, InputAction primaryInput, InputAction secondaryInput);

    // Simple helper function that handles finding all enemies in a trigger collider, damaging and knocking them
    public static void DamageInCollider(Collider2D collider, float baseDamage, Vector2 kb)
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D { layerMask = LayerMask.GetMask("Enemy"), useLayerMask = true };
        collider.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
            enemy.Damage((int) Math.Round(baseDamage));
            rb2d.velocity += kb;
        }
    }

    // Simple helper function that handles finding all enemies in a trigger collider, damaging and calculating kb for each enemy
    public static void DamageInCollider(Collider2D collider, float baseDamage, Func<Enemy, Vector2> kb)
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D { layerMask = LayerMask.GetMask("Enemy"), useLayerMask = true };
        collider.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
            enemy.Damage((int) Math.Round(baseDamage));
            rb2d.velocity += kb.Invoke(enemy);
        }
    }
}
