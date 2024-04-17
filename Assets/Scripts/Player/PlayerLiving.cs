using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour, Living
{
    // Health
    [SerializeField] private int maxHealth;
    private int health;
    public event Living.HealthChange onHealthChange;
    
    public delegate void WeaponChange(WeaponItem item, int index);
    public event WeaponChange onWeaponChange;

    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;

    private PlayerActions actions;
    private InputAction primaryAttack;
    private InputAction secondaryAttack;

    private ItemStack[] inventory = new ItemStack[3];

    [SerializeField] private WeaponItem[] weapons = new WeaponItem[2];
    private Weapon[] weaponObjects = new Weapon[2];
    private int equippedWeaponIndex = 0;
    private InputAction firstWeapon;
    private InputAction secondWeapon;

    void Awake()
    {
        InitializePlayer();
    }

    //constructor
    private void InitializePlayer()
    {
        health = maxHealth;

        actions = new PlayerActions();
        primaryAttack = actions.Attacks.PrimaryAttack;
        secondaryAttack = actions.Attacks.SecondaryAttack;

        firstWeapon = actions.Inventory.FirstWeapon;
        secondWeapon = actions.Inventory.SecondWeapon;

        firstWeapon.performed += (ctx) => { if (weapons[0] != null) EquipWeapon(weapons[0]); };
        secondWeapon.performed += (ctx) => { if (weapons[1] != null) EquipWeapon(weapons[1]); };

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
    public int DamageDealt() => 5;
    public void Heal(int healthHealed)
    {
        health = Math.Max(health + Math.Max(1, healthHealed), maxHealth);
        onHealthChange?.Invoke(health);
    }

    public void Damage(int amount)
    {
        if (currentIframes > 0) return;
        health = Math.Max(health - Math.Max(1, amount), 0);
        onHealthChange?.Invoke(health);
        currentIframes = iframes;
    }

    public ItemStack[] GetItems()
    {
        return this.inventory;
    }

    public WeaponItem GetEquipped()
    {
        return weapons[equippedWeaponIndex];
    }

    public WeaponItem[] GetWeapons()
    {
        return weapons;
    }

    // Changes the weapon at the given index to the supplied one
    public void ChangeWeapon(WeaponItem item, int index)
    {
        if (weapons[index] != null)
        {
            weaponObjects[index].OnUnequip(this, primaryAttack, secondaryAttack);
            Destroy(weaponObjects[index].gameObject);
            
            weaponObjects[index] = null;
            weapons[index] = null;
        }

        GiveWeapon(item);
        if (equippedWeaponIndex == index) EquipWeapon(item);

        onWeaponChange?.Invoke(item, index);
    }

    // Gives the player the weapon, if they have room. Returns false if no room is found or the player already has the same weapon equipped
    public bool GiveWeapon(WeaponItem weapon)
    {
        if (weapons.Length > 2) return false;
        if (Array.IndexOf(weapons, weapon) != -1) return false;
        int index = Array.IndexOf(weapons, null);
        weapons[index] = weapon;
        weaponObjects[index] = weapon.GetOrCreateWeapon(this);

        onWeaponChange?.Invoke(weapon, index);

        return true;
    }

    // Checks to see if the player has weapon, and makes them equip it as their active weapon
    public bool EquipWeapon(WeaponItem weapon)
    {
        int index = Array.IndexOf(weapons, weapon);
        if (index == -1) return false;

        weaponObjects[equippedWeaponIndex].OnUnequip(this, primaryAttack, secondaryAttack);
        weaponObjects[equippedWeaponIndex].gameObject.SetActive(false);
        equippedWeaponIndex = index;
        weaponObjects[equippedWeaponIndex].gameObject.SetActive(true);
        weaponObjects[equippedWeaponIndex].OnEquip(this, primaryAttack, secondaryAttack);

        return true;
    }

    public int GetEquippedIndex()
    {
        return equippedWeaponIndex;
    }
}