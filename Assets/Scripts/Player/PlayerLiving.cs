using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour, Living
{
    public static PlayerLiving Instance { protected set; get; }

    // Health
    [SerializeField] 
    private int maxHealth;
    private int health;
    public event Living.HealthChange onHealthChange;

    // Currency
    public int goldCount { get; protected set; }

    // Invulnerability
    [SerializeField]
    private float iframes = 1.5f;
    private float currentIframes = 0f;

    // Inputs
    private PlayerActions actions;
    private InputAction primaryAttack;
    private InputAction secondaryAttack;
    private InputAction switchToFirstWeapon;
    private InputAction switchToSecondWeapon;

    // Inventory
    [SerializeField] 
    private EquippableItem[] accessories = new EquippableItem[3];
    private Item[] inventory = new Item[5];

    // Weapons
    [SerializeField] 
    private WeaponItem[] weapons = new WeaponItem[2] { null, null };
    private Weapon[] weaponObjects = new Weapon[2] { null, null };
    private int equippedWeapon = 0;

    // Events
    public delegate void WeaponChange(WeaponItem item, int index);
    public event WeaponChange onWeaponChange;

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        Instance = this;
        InitializePlayer();
    }

    //constructor
    private void InitializePlayer()
    {
        health = maxHealth;

        actions = new PlayerActions();
        primaryAttack = actions.Attacks.PrimaryAttack;
        secondaryAttack = actions.Attacks.SecondaryAttack;

        switchToFirstWeapon = actions.Inventory.FirstWeapon;
        switchToSecondWeapon = actions.Inventory.SecondWeapon;

        switchToFirstWeapon.performed += (ctx) => { if (weapons[0] != null) EquipWeapon(weapons[0]); };
        switchToSecondWeapon.performed += (ctx) => { if (weapons[1] != null) EquipWeapon(weapons[1]); };

        // ugly ass weapon init, could probs be done better
        for (int i = 0; i < weapons.Length; i++)
        {
            WeaponItem weapon = weapons[i];
            if (weapon == null) continue;

            onWeaponChange?.Invoke(weapon, i);

            weaponObjects[i] = weapon.GetOrCreateWeapon(this);
            if (i != 0) weaponObjects[i].gameObject.SetActive(false);
            else weaponObjects[i].OnEquip(this, primaryAttack, secondaryAttack);
        }
    }

    void OnEnable()
    {
        actions.Enable();
    }

    void OnDisable()
    {
        actions.Disable();
    }

    void Update()
    {
        if (currentIframes > 0) currentIframes -= Time.deltaTime;
    }

    public int Health() => health;
    public int MaxHealth() => maxHealth;
    public int GetStrength() => 5;
    public void Heal(int healthHealed)
    {
        health = Math.Max(health + Math.Max(1, healthHealed), maxHealth);
        onHealthChange?.Invoke(health);
    }

    public void Damage(int amount)
    {
        if (currentIframes > 0) return;
        amount = accessories.Aggregate(amount, (acc, x) => Math.Max(1, x.ReduceDamage(acc)));
        health = Math.Max(health - Math.Max(1, amount), 0);
        onHealthChange?.Invoke(health);
        currentIframes = iframes;
    }

    public Item[] GetItems()
    {
        return this.inventory;
    }

    public WeaponItem GetEquippedWeapon()
    {
        return weapons[equippedWeapon];
    }

    public WeaponItem[] GetWeapons()
    {
        return weapons;
    }

    public bool GiveWeapon(WeaponItem weapon)
    {
        if (Array.IndexOf(weapons, weapon) != -1) return false;
        int index = Array.IndexOf(weapons, null);
        if (index == -1) return false;

        weapons[index] = weapon;
        weaponObjects[index] = weapon.GetOrCreateWeapon(this);
        onWeaponChange?.Invoke(weapon, index);

        return true;
    }

    public bool EquipWeapon(WeaponItem weapon, bool overrideRoom = false)
    {
        if (!overrideRoom)
        {
            int index = Array.IndexOf(weapons, weapon);
            if (index == -1) return false;
            weaponObjects[equippedWeapon].OnUnequip(this, primaryAttack, secondaryAttack);
            weaponObjects[equippedWeapon].gameObject.SetActive(false);
            equippedWeapon = index;
            weaponObjects[equippedWeapon].gameObject.SetActive(true);
            weaponObjects[equippedWeapon].OnEquip(this, primaryAttack, secondaryAttack);
        }
        else
        {
            if (weapons[equippedWeapon] != null)
            {
                weaponObjects[equippedWeapon].OnUnequip(this, primaryAttack, secondaryAttack);
                Destroy(weaponObjects[equippedWeapon].gameObject);
            }

            weapons[equippedWeapon] = weapon;
            weaponObjects[equippedWeapon] = weapon.GetOrCreateWeapon(this);
            weaponObjects[equippedWeapon].OnEquip(this, primaryAttack, secondaryAttack);
            onWeaponChange?.Invoke(weapon, equippedWeapon);
        }

        return true;
    }

    public WeaponItem GetWeaponAt(int index)
    {
        return weapons[index];
    }

    public int GetEquippedWeaponIndex()
    {
        return equippedWeapon;
    }

    public EquippableItem[] GetAccessories()
    {
        return accessories;
    }

    public void GiveAccessory(EquippableItem item)
    {

    }
    
    public void AddGold(int amount)
    {
        goldCount += Math.Max(0, amount);
    }

    public void SubtractGold(int amount)
    {
        goldCount -= Math.Max(0, amount);
    }

    public void SetGold(int amount)
    {
        goldCount = Math.Max(0, amount);
    }
}