using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HolySwordItem : WeaponItem
{
    public override string weaponName => "Holy Sword";
    public override float primaryCooldown => .7f;
    public override float secondaryCooldown => .5f;

    public override void DefineColliders(BoxCollider2D primary, BoxCollider2D secondary)
    {
        primary.size = new Vector2(.4f, .9f);
        secondary.size = new Vector2(.8f, .2f);
    }

    public override void PrimaryAttack(PlayerLiving player, BoxCollider2D box, WeaponHandler handler)
    {
        WeaponItem.DamageInCollider(box, 3f, (Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position)).normalized * 35f);
    }

    public override void SecondaryAttack(PlayerLiving player, BoxCollider2D box, WeaponHandler handler)
    {
        WeaponItem.DamageInCollider(box, 1f, (Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position)).normalized * 55f);
    }

    
}