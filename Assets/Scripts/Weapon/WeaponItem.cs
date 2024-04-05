using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : Item
{
    public abstract string weaponName { get; }
    public abstract float primaryCooldown { get; }
    public abstract float secondaryCooldown { get; }

    public virtual void PrimaryAttack(PlayerLiving player, BoxCollider2D collider, WeaponHandler handler)
    {
        
    }

    public virtual void SecondaryAttack(PlayerLiving player, BoxCollider2D collider, WeaponHandler handler)
    {
    }

    public abstract void DefineColliders(BoxCollider2D primary, BoxCollider2D secondary);

    // Simple helper function that handles finding all enemies in a trigger collider, damaging and knocking them
    public static void DamageInCollider(BoxCollider2D collider, float baseDamage, Vector2 kb)
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
}