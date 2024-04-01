An instance of an Item can be an EquippableItem, and an instance of an EquippableItem can be a WeaponItem. A WeaponItem can provide passive changes, just like EquippableItems, but also have Primary and Secondary attacks.

A WeaponItem can be shaped as follows:
```c#
public abstract class WeaponItem
{
    public abstract PlayerClass ForClass { get; }
    public abstract float PrimaryCooldown { get; }
    public abstract float SecondaryCooldown { get; }
    
    public abstract void Primary(Player player);
    public abstract void Secondary(Player player);
}
```
It provides information like its cooldown, the class that can use it, and functions that fire for attacks.

```c#
public class IronLanceWeapon : WeaponItem
{
    public override PlayerClass ForClass { get { return PlayerClass.Warrior } }
    public override float PrimaryCooldown { get { return 1f } }

    public override void Primary(Player player)
    {
        List<RaycastHit2D> results = new List<RaycastHit2D>();
        Physics2D.BoxCast(
            player.transform.position + player.GetMouseVector() * 5, new Vector2(3, 6), player.GetMouseAngle(),
            Vector2.zero, null, ref results
        );
        foreach (RaycastHit2D hit in results)
        {
            Living living = hit.transform.GetComponent<Living>();
            if (living == null) continue;
            float damageDealt = player.CalculatePower() * .3f
            living.TakeDamage(damageDealt);
            // Get the player's armour, accessories, etc and invoke OnPlayerAttack
            player.GetArmour()?.OnPlayerAttack(player, living, damageDealt);
        }

        // Swinging animation, particles and SFX can be called here. Hitbox 
        // logic can also be delayed using animation events
        // WeaponItem#Particles and WeaponItem#SFX will be called here too
    }
}
```
This example weapon creates a 3 by 6 box (tall and narrow) 5 units towards the mouse from the player, rotated to also face the mouse when performing the primary attack. For every collider that has the Living component hit, perform the necessary damage calculations and apply the damage.