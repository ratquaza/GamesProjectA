using System;
using System.Collections.Generic;
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

    [SerializeField] private Rigidbody2D rb2d;

    private PlayerActions actions;
    private InputAction primaryAttack;
    private InputAction secondaryAttack;
    [SerializeField] private WeaponItem equippedWeapon;

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

        if (equippedWeapon) OnWeaponEquip(equippedWeapon);
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
        Debug.Log($"Player HP: {health}");
        onHealthChange?.Invoke(health);
    }

    public void Damage(int amount)
    {
        if (currentIframes > 0) return;
        health = Math.Max(health - Math.Max(1, amount), 0);
        Debug.Log($"Player HP: {health}");
        onHealthChange?.Invoke(health);
        currentIframes = iframes;
    }

    public void EquipWeapon(WeaponItem item)
    {
        if (equippedWeapon != null) OnWeaponUnequip(equippedWeapon);
        equippedWeapon = item;
        OnWeaponEquip(item);
    }

    private void OnWeaponEquip(WeaponItem item)
    {
        item.GetOrCreateWeapon(this).OnEquip(this, item, primaryAttack, secondaryAttack);
    }

    private void OnWeaponUnequip(WeaponItem item)
    {
        Weapon weapon = item.GetOrCreateWeapon(this);
        weapon.OnUnequip(this, item, primaryAttack, secondaryAttack);
        Destroy(weapon.gameObject);
    }
}