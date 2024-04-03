using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Android;

public class Enemy : MonoBehaviour
{
    private float enemyHealth;
    private float goldDropAmount;
    [SerializeField] private int damage;
    [SerializeField] private float projectileCooldown;


    [Header("Navigation Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    //NOTE: IF YOU WANT TO TUNE THE SPEED AND ACCELERATION SETTINGS OF THE ENEMY, MODIFY THE NAVMESHAGENT COMPONENT. (Can do through code or inspector)

    void Awake()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        enemyHealth = 100;
        goldDropAmount = 10;
        damage = 10;
    }


    private void Start()
    {
        if(target != null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        //initialize agent 
        agent = GetComponent<NavMeshAgent>();

        //sets agent properties 
        //don't touch: recommomended settings from NavMeshPlus -->   https://github.com/h8man/NavMeshPlus
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    public int GetDamage()
    {
        return damage;
    }

    private void Update()
    {
        setTarget(target);
    }

    public void TakeDamage(float playerDamage)
    {
        enemyHealth -= damage;
        if(enemyHealth <= 0){
            Destroy(gameObject);
            Debug.Log("Enemy Died");
        }
    }

    //set agent's target to any transform (default: player)
    public void setTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }

    void EnemyAttack()
    {
        //TODO: Attack logic (Using navmeshagent's inbuilt stopping distance)
    }

}
