using UnityEngine;

public abstract class WeaponItem : Item
{
    public abstract string weaponName { get; }
    public abstract float primaryCooldown { get; }
    public abstract float secondaryCooldown { get; }
    public abstract Sprite sprite { get; }
    public abstract PlayerClass forClass { get; }

    public virtual void PrimaryAttack(PlayerLiving player, BoxCollider2D collider)
    {
        
    }

    public virtual void SecondaryAttack(PlayerLiving player, BoxCollider2D collider)
    {
    }

    public abstract void DefinePrimaryCollider(BoxCollider2D bc2d);
    public abstract void DefineSecondaryCollider(BoxCollider2D bc2d);
}