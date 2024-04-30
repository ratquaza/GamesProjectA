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
    public event Action<int> OnGoldCountChanged;

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

        SetGold(1000);

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

    public void SetIFrames(float giveIFrames)
    {
        currentIframes = giveIFrames;
    }
    public float GetIFrames()
    {
        return iframes;
    }

    public float Health() => health;
    public float MaxHealth() => maxHealth;
    public float GetStrength() => accessories.Aggregate(3, (acc, a) => a == null ? acc : Math.Max(1, a.ModifyStrength(acc)));
    public void Heal(float healthHealed)
    {
        health = (int) Math.Max(health + Math.Max(1, healthHealed), maxHealth);
        onHealthChange?.Invoke(health);
    }

    public void TakeDamage(float amount, bool applyIframes = true)
    {
        if (currentIframes > 0 && applyIframes == true) return;
        string debugString = amount + " > ";
        amount = accessories.Aggregate(amount, (acc, x) => x == null ? acc : Math.Max(1, x.ModifyDamage((int)acc)));
        debugString += amount;
        Debug.Log(debugString);
        TakeDamageFinal(amount);
    }

    public void TakeDamageFinal(float amount)
    {
        health = (int) Math.Max(health - Math.Max(1, amount), 0);
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
    
    public void AddGold(int amount)
    {
        goldCount += Math.Max(0, amount);
        OnGoldCountChanged?.Invoke(goldCount);
    }

    public void SubtractGold(int amount)
    {
        goldCount -= Math.Max(0, amount);
        OnGoldCountChanged?.Invoke(goldCount);
    }

    public void SetGold(int amount)
    {
        goldCount = Math.Max(0, amount);
        OnGoldCountChanged?.Invoke(goldCount);
    }
}