using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour, Living
{
    // Health
    [SerializeField] private int maxHealth;
    private int health;
    public delegate void HealthChange(int health);
    public event HealthChange onHealthChange;
    
    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;
    [SerializeField] private Rigidbody2D rb2d;

    // Attack actions
    [SerializeField] private WeaponHandler handler;
    private PlayerActions actions;
    private float primaryAttackCooldown = 0f;
    private InputAction primaryAttack;
    private float secondaryAttackCooldown = 0f;
    private InputAction secondaryAttack;

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

        primaryAttack.performed += ctx => AttemptPrimaryAttack();
        secondaryAttack.performed += ctx => AttemptSecondaryAttack();
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
        if (primaryAttackCooldown > 0) primaryAttackCooldown -= Time.deltaTime;
        if (secondaryAttackCooldown > 0) secondaryAttackCooldown -= Time.deltaTime;
    }

    private void AttemptPrimaryAttack()
    {
        if (primaryAttackCooldown > 0) return;
        handler.PrimaryAttack(this);
        primaryAttackCooldown = handler.GetWeapon().primaryCooldown;
    }

    private void AttemptSecondaryAttack()
    {
        if (secondaryAttackCooldown > 0) return;
        handler.SecondaryAttack(this);
        secondaryAttackCooldown = handler.GetWeapon().secondaryCooldown;
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
}