using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerHealth;
    [SerializeField] private int goldCount;
    [SerializeField] private List<Item> items;

    
    //Invulnerability 
    [SerializeField] private float invulnerabilityDuration = 1.5f;
    private bool isInvulnerable = false;
    private float invulnerabilityTimer = 0f;


    void Awake()
    {
        InitializePlayer();
    }

    //constructor
    private void InitializePlayer()
    {
        playerHealth = 100;
        goldCount = 0;
        items = new List<Item>();
    }

    //while player is collided with enemy, take damage depending on couroutine's invulnerabilityDuration
    void OnCollisionStay2D(Collision2D collision)
    {
        if (!isInvulnerable && collision.gameObject.GetComponent<Enemy>() != null && collision.contactCount > 0)
        {
            StartCoroutine(TakeDamageCoroutine(collision));
            Debug.Log("Player Health: " + playerHealth);
        }
    }

    IEnumerator TakeDamageCoroutine(Collision2D collision)
    {
        isInvulnerable = true;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        int damageTaken = enemy.GetDamage();
        TakeDamage(damageTaken);

        yield return new WaitForSeconds(invulnerabilityDuration);

        isInvulnerable = false;
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