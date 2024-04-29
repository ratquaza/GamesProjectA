using System;
using UnityEngine;

public class ArmourItem : Item
{
    [Tooltip("Reduces armour through percentage.")]
    [SerializeField] protected float armourPercentage;
    [Tooltip("Reduces armour with a flat rate.")]
    [SerializeField] protected int armourValue;

    public int ReduceValue(int value)
    {
        return (int) Math.Round((value - armourValue) * (1f - armourPercentage));
    }
}