using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int goldCount;
    [SerializeField] private List<Item> items;
    private WeaponItem equippedWeapon = new HolySword();
    public delegate void HealthChange(int health);
    public event HealthChange onHealthChange;
    
    //Invulnerability
    [SerializeField] private float iframes = 1.5f;
    private float currentIframes = 0f;
    [SerializeField] private Rigidbody2D rb2d;

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
    }

    void Update()
    {
        if (currentIframes > 0) currentIframes -= Time.deltaTime;
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
}