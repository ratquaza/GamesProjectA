using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int goldCount;
    [SerializeField] private List<Item> items;


    void Awake()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        playerHealth = 100;
        goldCount = 0;
        items = new List<Item>();
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            int damageTaken = enemy.GetDamage();
            TakeDamage(damageTaken);

            Debug.Log("Player Health: " + playerHealth + "Damage Taken: " + damageTaken);
        }
    }

    public void TakeDamage(int damageTaken)
    {
        playerHealth -= damageTaken;
    }

    public void Heal(int healthHealed)
    {
        playerHealth += healthHealed;
    }
}