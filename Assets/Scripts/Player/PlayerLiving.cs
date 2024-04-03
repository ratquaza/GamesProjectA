using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLiving : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int goldCount;
    [SerializeField] private List<Item> items;
    [SerializeField] private WeaponHandler handler;
    public delegate void HealthChange(int health);
    public event HealthChange onHealthChange;
    
    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;
    [SerializeField] private Rigidbody2D rb2d;

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
        playerHealth = maxHealth;
        goldCount = 0;
        items = new List<Item>();

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

    //while player is collided with enemy and invulnerability is <= 0, take damage
    void OnCollisionStay2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (currentIframes <= 0 && enemy != null && collision.contactCount > 0)
        {
            int damageTaken = enemy.GetDamage();
            TakeDamage(damageTaken, enemy);
            currentIframes = iframes;
            Debug.Log("Player Health: " + playerHealth);
        }
    }

    public void TakeDamage(int damageTaken, Enemy source = null)
    {
        playerHealth -= Math.Max(damageTaken, 0);
        onHealthChange?.Invoke(playerHealth);
    }

    public void Heal(int healthHealed)
    {
        playerHealth += Math.Max(healthHealed, 0);
        onHealthChange?.Invoke(playerHealth);
    }

    public int MaxHealth()
    {
        return maxHealth;
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
}