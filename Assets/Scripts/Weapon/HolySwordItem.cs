using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolySwordItem : WeaponItem
{
    public override string weaponName => "Holy Sword";
    public override float primaryCooldown => 1f;
    public override float secondaryCooldown => 3f;

    public override void DefineColliders(BoxCollider2D primary, BoxCollider2D secondary)
    {
        primary.size = new Vector2(.3f, 1f);
        secondary.size = new Vector2(.8f, .2f);
    }

    public override void PrimaryAttack(PlayerLiving player, BoxCollider2D box)
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D { layerMask = LayerMask.GetMask("Enemy"), useLayerMask = true };

        box.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
            rb2d.velocity += (Vector2) (enemy.transform.position - player.transform.position).normalized * 30f;
        }
    }

    public override void SecondaryAttack(PlayerLiving player, BoxCollider2D box)
    {
    }

    
}