## Player Class
```c#
enum EquipmentType
{
    Weapon,
    Armour,
    Accessory
}

abstract class EquippableItem : Item 
{
    public abstract EquipmentType Type { get; }

    public virtual void OnEquip(Player player) {};
    public virtual void OnUnequip(Player player) {};
    
    public virtual float OnPlayerDamaged(Player player, Living attacker, float amount) { return amount; }
    public virtual float OnGetDamageDealt(Player player, float baseAmount) { return amount; }
    public virtual void OnPlayerAttacked(Player player, Living target, float finalDamageDealt) { return amount; }
    public virtual Vector2 OnPlayerDash(Player player, Vector2 dir) { return dir; }
}
```
An EquippableItem, which extends some base Item class, has an abstract property that must be overriden, and virtual methods they can override to define how they behave when equipped.
```c#
class Player : Living
{
    public List<Item> inventory = new List<Item>();
    
    public EquippableItem? equippedArmour = null;

    void EquipArmour(EquippableItem? item)
    {
        if (item != null && (item.Type != EquipmentType.Armour || !inventory.Contains(item))) return;
        if (equippedArmour != null) equippedArmour.OnUnequip(this);
        equippedArmour = item;
        if (equippedArmour != null) equippedArmour.OnEquip(this);
    }
}
```
The player's EquipArmour function checks to see if the item passed in is of ItemType Armour, and if the player has the item present in their inventory. If one is not true, the function ends early.

As the function runs, the relevant OnEquip and OnUnequip functions are called to add and remove event handlers.
```c#
class Player : Living
{
    private float playerBaseDamage = 5f;
    public float health = 100f;

    // Handle incoming damage and return the total damage taken
    override float TakeDamage(float amount, Living attacker)
    {
        if (equippedArmour != null) amount = equippedArmour.OnPlayerDamaged(this, attacker, amount);
        health = Math.Max(0, health - amount);
        return amount;
    }

    // Calculate the damage the player can deal right now 
    override float GetDamageDealt()
    {
        float baseAmount = playerBaseDamage;
        if (equippedArmour != null) baseAmount = equippedArmour.OnGetDamageDealt(this, baseAmount);
        return baseAmount;
    }
}
```
When certain actions are performed on the player, equipment like armour and accessories are able to modifiy values and perform additional actions after calculations.

An example armour that has lifesteal properties:
```csharp
public class VampireArmour : EquippableItem
{
    public override EquipmentType Type { 
        get { 
            return EquipmentType.Armour 
        }
    }

    public override float OnPlayerDamaged(Player player, Living attacker, float amount)
    { 
        // Flat 30% damage reduction
        return Math.Floor(amount * .7f);
    }

    public override float OnGetDamageDealt(Player player, float amount)
    {
        // 10% Damage increase
        return Math.Floor(amount * 1.1f);
    }

    public override void OnPlayerAttacked(Player player, Living target, float damageDealt)
    {
        // 1 in 5 chance for player to recover 10% of the damage they dealt
        if (Random.Range(0, 5) == 0) player.health = Math.Min(player.maxHealth, player.health + Math.Floor(damageDealt * .1f));
    }
}
```