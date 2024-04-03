using UnityEngine;

public abstract class WeaponItem : Item
{
    public abstract string name { get; }
    public abstract float primaryCooldown { get; }
    public abstract float secondaryCooldown { get; }
    public abstract Sprite sprite { get; }
    public abstract PlayerClass forClass { get; }

    public virtual void PrimaryAttack(Player player)
    {
    }

    public virtual void SecondaryAttack(Player player)
    {
    }
}