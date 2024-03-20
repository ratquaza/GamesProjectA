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
    [SerializeField] private float projectileCooldown;

    [Header("Navigation Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;

    //NOTE: IF YOU WANT TO TUNE THE SPEED AND ACCELERATION SETTINGS OF THE ENEMY, MODIFY THE NAVMESHAGENT COMPONENT. (Can do through code or inspector)

    private void Start()
    {
        //set default target to player
        target = GameObject.FindGameObjectWithTag("Player").transform;

        //initialize agent 
        agent = GetComponent<NavMeshAgent>();

        //sets agent properties 
        //don't touch: recommomended settings from NavMeshPlus -->   https://github.com/h8man/NavMeshPlus
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }

    private void Update()
    {
        setTarget(target);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Debug.Log(GetComponent<BoxCollider2D>().name + " collided with " + collision.gameObject.name);

        //TODO: Enemy, Player Collision Logic
    }

    private void TakeDamage(float damageDone)
    {
        enemyHealth -= damageDone;
    }

    //set agent's target to any transform (default: player)
    public void setTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }
}
