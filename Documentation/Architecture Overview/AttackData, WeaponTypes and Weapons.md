An instance of an Item can be an EquippableItem, and an instance of an EquippableItem can be a WeaponItem.A WeaponItem can provide passive changes, just like EquippableItems, but also are required to provide a WeaponType, as this is their 'category', and additional data if they so choose.

# Approach A - Hitbox Collision via code
This approach uses the Physics2D class to perform raycasting methods when performing attacks, with every WeaponType having its own constants used for the sizing of hitboxes while instances that carry these WeaponTypes provide additional data on attacks.

An example of a WeaponType can be a Lance, and how it performs its attacks:
```c#
public abstract class WeaponType
{
    public static readonly LanceWeaponType LANCE = new LanceWeaponType();

    public abstract void PrimaryAttack(WeaponItem weapon, Player player, float knockbackGrowth = 1f);
    public abstract void SecondaryAttack(WeaponItem weapon, Player player, float knockbackGrowth = 1f);

    private class LanceWeaponType : WeaponType
    {
        public override void PrimaryAttack(WeaponItem weapon, Player player, float kbg = 1f)
        {
            // Creates a 3 by 6 box (tall and narrow) 5 units towards the mouse from the player, rotated
            // to also face the mouse
            List<RaycastHit2D> results = new List<RaycastHit2D>();
            Physics2D.BoxCast(
                player.transform.position + player.GetMouseVector() * 5, new Vector2(3, 6), player.GetMouseAngle(),
                Vector2.zero, null, ref results
            );
            // For every enemy hit, perform damage calculations
            foreach (RaycastHit2D hit in results)
            {
                Living living = hit.transform.GetComponent<Living>();
                if (living == null) continue;
                living.TakeDamage(player.GetDamageDealt() * .5f);
                // If the WeaponItem has additional behaviour, it's called here
                weapon.OnHitEffect(living);
                // Apply knockback here too, etc.
            }

            // Swinging animation, particles and SFX can be called here. Hitbox 
            // logic can also be delayed using animation events
            // WeaponItem#Particles and WeaponItem#SFX will be called here too
        }

        public override void SecondaryAttack(WeaponItem weapon, Player player, float kbg = 1f)
        {
            // Secondary attack logic
        }
    }
}
```
Then, a WeaponItem would be shaped like so.
```c#
public abstract class WeaponItem : EquippableItem
{
    public override EquipmentType Type
    { 
        get { return EquipmentType.Weapon }
    }

    public abstract WeaponType WeaponType { get; }

    public virtual void PrimaryAttack(Player player)
    {
        WeaponType.PrimaryAttack(this, player, 1f);
    }

    public virtual void SecondaryAttack(Player player)
    {
        WeaponType.SecondaryAttack(this, player, 1f);
    }

    public virtual void OnHitEffect(Living hit) {}
    public virtual Sprite[]? Particles() { return null; }
    public virtual AudioClip[]? SFX() { return null; }
}
```
Since this class is abstract, we can make a simple Lance WeaponItem like so:
```c#
public class IronLance : WeaponItem
{
    public override WeaponType WeaponType
    {
        get { return WeaponType.LANCE }
    }

    // LanceWeaponItem extends WeaponItem, which extends EquippableItem.
    // EquippableItem's function OnGetDamageDealt is fired whenever Player#GetDamageDealt is
    // See [Passive Effects of Equipment.md]
    public override float OnGetDamageDealt(Player player, float baseAmount) { 
        return amount + 10; 
    }
}
```
If there are no additional effects needed, then this Lance Weapon is complete. 

Another Lance Weapon could be designed like so: 
```c#
public class CrystalLance : WeaponItem
{
    public override WeaponType WeaponType
    {
        get { return WeaponType.LANCE }
    }
    
    public override void SecondaryAttack(Player player)
    {
        WeaponType.SecondaryAttack(this, player, 2f);
    }

    public override void OnHitEffect(Living hit)
    {
        // 1 in 5 chance to apply a debuff to enemies when hit.
        if (Random.Range(0, 5) != 0) return;
        hit.AddDebuff(new FrozenCrystalDebuff());
    }
}
```
Both IronLance and CrystalLance are separate items yet are also both Lances. They both ultimately call the same PrimaryAttack and SecondaryAttack, 

# Approach B - Hitbox Collision via Trigger Colliders
Instead of having the hitboxes performed via Physics2D, the Player GameObject can hold 2 Colliders on them that are triggers. When a player equips a new WeaponItem, the WeaponItem changes the size, position, or even the type of Colliders entirely. 

```c#
public abstract class WeaponType
{
    public static readonly LanceWeaponType LANCE = new LanceWeaponType();

    public abstract void DefinePrimaryAttackTrigger(WeaponItem weapon, Player player);
    public abstract void DefineSecondaryAttackTrigger(WeaponItem weapon, Player player);

    public abstract void HandlePrimaryAttack(WeaponItem weapon, Player player, float knockbackGrowth = 1f);
    public abstract void HandleSecondaryAttack(WeaponItem weapon, Player player, float knockbackGrowth = 1f);

}
```