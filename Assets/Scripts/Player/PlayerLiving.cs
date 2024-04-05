using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour, Living
{
    // Health
    [SerializeField] private int maxHealth;
    private int health;
    
    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;
    [SerializeField] private Rigidbody2D rb2d;

    // Attack actions
    [SerializeField] private WeaponHandler handler;
    private PlayerActions actions;
   
    private InputAction primaryAttack;
    private InputAction secondaryAttack;
    public event Living.HealthChange onHealthChange;

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

        primaryAttack.performed += ctx => handler.PrimaryAttack(this);
        secondaryAttack.performed += ctx => handler.SecondaryAttack(this);
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
}