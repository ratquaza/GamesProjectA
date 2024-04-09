using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour, Living
{
    // Health
    [SerializeField] private int maxHealth;
    private int health;
    public event Living.HealthChange onHealthChange;
    
    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;

    private PlayerActions actions;
    private InputAction primaryAttack;
    private InputAction secondaryAttack;
    [SerializeField] private WeaponItem equippedWeaponItem;
    private Weapon equippedWeaponObject;
    private int weaponIndex = 0;

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

        if (equippedWeaponItem) OnWeaponEquip(equippedWeaponItem);
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
        if (Input.GetKeyDown(KeyCode.G))
        {
            Dictionary<string, Item> items = ItemDatabase.Instance.Items;
            weaponIndex = (weaponIndex + 1) % items.Count;
            EquipWeapon(items.ElementAt(weaponIndex).Value as WeaponItem);
        }
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

    public void EquipWeapon(WeaponItem item)
    {
        if (equippedWeaponItem != null) OnWeaponUnequip();
        OnWeaponEquip(item);
    }

    private void OnWeaponEquip(WeaponItem item)
    {
        equippedWeaponItem = item;
        equippedWeaponObject = item.GetOrCreateWeapon(this);
        equippedWeaponObject.OnEquip(this, primaryAttack, secondaryAttack);
    }

    private void OnWeaponUnequip()
    {
        equippedWeaponObject.OnUnequip(this, primaryAttack, secondaryAttack);
        DestroyImmediate(equippedWeaponObject.gameObject);
        equippedWeaponItem = null;
    }
}