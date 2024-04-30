using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Equippable", menuName = "Items/Equippable", order = 0)]
public class EquippableItem : Item
{
    [Tooltip("Reduces armour through percentage. Applied second.")]
    [SerializeField] 
    protected float armourPercentage = 1f;

    [Tooltip("Reduces armour with a flat rate. Applied first.")]
    [SerializeField] 
    protected int armourValue;

    [Tooltip("Increases strength through percentage. Applied second.")]
    [SerializeField] 
    protected float strengthPercentage = 1f;

    [Tooltip("Increases strength with a flat rate. Applied first.")]
    [SerializeField] 
    protected int strengthValue;

    public virtual int ModifyDamage(int value)
    {
        return (int) Math.Round((value - armourValue) * armourPercentage);
    }

    public virtual int ModifyStrength(int value)
    {
        return (int) Math.Round((value + strengthValue) * strengthPercentage);
    }

    public virtual void OnEquip(PlayerLiving player) {}
    public virtual void OnUnequip(PlayerLiving player) {}
}