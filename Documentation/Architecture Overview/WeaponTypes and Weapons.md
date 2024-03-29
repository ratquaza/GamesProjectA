An instance of an Item can be an EquippableItem, and an instance of an EquippableItem can be a WeaponItem. A WeaponItem can provide passive changes, just like EquippableItems, but also have Primary and Secondary attacks.

# Approach A - Hitbox Collision via code
This approach uses the Physics2D class to perform raycasting methods when performing attacks.

An example of a WeaponType can be a Lance, and how it performs its attacks:
```c#
public abstract class WeaponItem
{
    public abstract float PrimaryCooldown { get; }
    public abstract float SecondaryCooldown { get; }
    // Return type is every living hit
    public abstract void Primary(Player player);
    public abstract void Secondary(Player player);
}

public class IronLanceWeapon : WeaponItem
{
    public override float PrimaryCooldown { get { return 1f } }
    public override float SecondaryCooldown { get { return 4f } }

    public override void Primary(Player player)
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
            float damageDealt = player.CalculatePower() * .3f
            living.TakeDamage(damageDealt);
            // Get the player's armour and invoke their attack handler
            // Also do this for accessories etc
            player.GetArmour()?.OnPlayerAttack(player, living, damageDealt);
        }

        // Swinging animation, particles and SFX can be called here. Hitbox 
        // logic can also be delayed using animation events
        // WeaponItem#Particles and WeaponItem#SFX will be called here too

        return living.ToArray();
    }

    public override void Secondary(Player player)
    {
        // Secondary attack logic
    }
}
```