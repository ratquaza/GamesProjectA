using System;
using UnityEngine;

public class EquipableItem : ScriptableObject
{
    [Tooltip("Reduces armour through percentage. Applied second.")]
    [SerializeField] 
    protected float armourPercentage;

    [Tooltip("Reduces armour with a flat rate. Applied first.")]
    [SerializeField] 
    protected int armourValue;

    [Tooltip("Increases strength through percentage. Applied second.")]
    [SerializeField] 
    protected float strengthPercentage;

    [Tooltip("Increases strength with a flat rate. Applied first.")]
    [SerializeField] 
    protected int strengthValue;

    [Tooltip("Is this a piece of Armour or an Accessory?")]
    [SerializeField]
    protected bool isArmour;

    public virtual int ReduceDamage(int value)
    {
        return (int) Math.Round((value - armourValue) * (1f - armourPercentage));
    }

    public virtual int IncreaseStrength(int value)
    {
        return (int) Math.Round((value + strengthValue) * (1f + strengthPercentage));
    }

    public bool IsArmour() => isArmour;

    public virtual void OnEquip(PlayerLiving player) {}
    public virtual void OnUnequip(PlayerLiving player) {}
}