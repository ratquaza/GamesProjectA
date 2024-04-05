using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Living
{
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private int damage;
    [SerializeField] private float projectileCooldown;
    [SerializeField] private int goldDropAmount;


    [Header("Navigation Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody2D rb2d;

    //NOTE: IF YOU WANT TO TUNE THE SPEED AND ACCELERATION SETTINGS OF THE ENEMY, MODIFY THE NAVMESHAGENT COMPONENT. (Can do through code or inspector)

    void Awake()
    {
        InitializeEnemy();
    }

    private void InitializeEnemy()
    {
        health = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target == null){
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }

        //initialize agent 
        agent = GetComponent<NavMeshAgent>();

        //sets agent properties 
        //don't touch: recommomended settings from NavMeshPlus -->   https://github.com/h8man/NavMeshPlus
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        rb2d.velocity *= .99f;
    }

    public void Damage(int damageDone)
    {
        health = Math.Max(0, health - damageDone);
    }

    public void Heal(int amount)
    {
        health = Math.Min(maxHealth, health + amount);
    }
    public int DamageDealt() => damage;
    public int Health() => health;
    public int MaxHealth() => maxHealth;

    //set agent's target to any transform (default: player)
    public void SetTarget(Transform target)
    {
        agent.SetDestination(target.position);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        PlayerLiving player = collision.gameObject.GetComponent<PlayerLiving>();
        if (player != null) player.Damage(DamageDealt());
    }
}