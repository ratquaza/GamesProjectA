using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Living
{
    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] private int damage;
    [SerializeField] private int goldDropAmount;
    [SerializeField] private float drag = 10f;


    [Header("Navigation Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody2D rb2d;
    public event Living.HealthChange onHealthChange;

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
        if (target == null) target = GameObject.FindGameObjectWithTag("Player").transform;
        rb2d.drag = drag;

        //initialize agent 
        agent = GetComponent<NavMeshAgent>();

        //sets agent properties 
        //don't touch: recommomended settings from NavMeshPlus -->   https://github.com/h8man/NavMeshPlus
        agent.updateRotation = false;
        agent.updateUpAxis = false;

    }
    
    void Update()
    {
        agent.SetDestination(target.position);
    }

    public void Damage(int damageDone)
    {
        health = Math.Max(0, health - damageDone);
        onHealthChange.Invoke(health);
        if (health == 0) Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        health = Math.Min(maxHealth, health + amount);
        onHealthChange.Invoke(health);
    }
    public int GetStrength() => damage;
    public int Health() => health;
    public int MaxHealth() => maxHealth;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        PlayerLiving player = collision.gameObject.GetComponent<PlayerLiving>();
        if (player != null) player.Damage(GetStrength());
    }
}