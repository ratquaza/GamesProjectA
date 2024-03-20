using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float enemyHealth;
    private float goldDropAmount;
    [SerializeField] private float projectileCooldown;

    [SerializeField] private float moveSpeed = 300f; 
    [SerializeField] private float stoppingDistance = 50f; //when the enemy stops following

    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D rb;

    void Awake()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            Debug.Log("Player GameObject found: " + playerObject.name);
            target = playerObject.transform;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        EnemyFollow();
    }

    private void EnemyFollow(){
        if (target != null)
        {
            //move towards target
            Vector2 directionToTarget = target.position - transform.position;
            if (directionToTarget.magnitude > stoppingDistance)
            {
                Vector2 moveDirection = directionToTarget.normalized * moveSpeed;
                rb.velocity = moveDirection;
            }
            //stop once in stoppingDistance
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        //if no target, stay still
        else{
            rb.velocity = Vector2.zero;
        }
    }

    private void TakeDamage(float damageDone){
        enemyHealth -= damageDone;
    }



}
