using UnityEngine;

public class HolySword : WeaponItem
{
    public override string name => "Holy Sword";

    public override float primaryCooldown => 1f;

    public override float secondaryCooldown => 3f;

    public override Sprite sprite => throw new System.NotImplementedException();

    public override PlayerClass forClass => PlayerClass.Warrior;
}