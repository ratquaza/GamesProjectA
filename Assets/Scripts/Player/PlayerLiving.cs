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

    private WeaponItem toEquip;
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
        if (toEquip != null)
        {
            OnWeaponEquip(toEquip);
            toEquip = null;
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
        toEquip = item;
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
        Destroy(equippedWeaponObject.gameObject);
        equippedWeaponItem = null;
        equippedWeaponObject = null;
    }
}