using UnityEngine;

public abstract class WeaponItem : Item
{
    public abstract string weaponName { get; }
    public abstract float primaryCooldown { get; }
    public abstract float secondaryCooldown { get; }

    public virtual void PrimaryAttack(PlayerLiving player, BoxCollider2D collider)
    {
        
    }

    public virtual void SecondaryAttack(PlayerLiving player, BoxCollider2D collider)
    {
    }

    public abstract void DefineColliders(BoxCollider2D primary, BoxCollider2D secondary);
}