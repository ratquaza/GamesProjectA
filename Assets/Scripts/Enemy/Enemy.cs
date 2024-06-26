using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, Living
{
    [SerializeField] private int maxHealth;
    private int health;
    private PlayerLiving player { get => PlayerLiving.Instance; }

    [SerializeField] private int damage;
    [SerializeField] private float drag = 10f;

    [SerializeField] private int enemyWeight = 20;
    [SerializeField] private int goldDropAmount;
    [SerializeField] private GameObject GoldCoinPrefab;
    [SerializeField] private GameObject SilverCoinPrefab;
    private float totalGoldWeight;

    public List<GameObject> coinPrefabs;

    [Header("Navigation Settings")]
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
        EnemyCoinDrops();

        health = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
    }

    private void EnemyCoinDrops()
    {
        coinPrefabs = new List<GameObject>();
        if (enemyWeight < 50)
        {
            coinPrefabs.Add(GoldCoinPrefab);
            coinPrefabs.Add(SilverCoinPrefab);
        }
        else if (enemyWeight >= 50)
        {
            coinPrefabs.Add(SilverCoinPrefab);
        }

    }

    private void Start()
    {
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
        if (player != null) agent.SetDestination(player.transform.position);
    }

    public void TakeDamage(float damageDone, bool applyIframes)
    {
        health = (int) Math.Max(0, health - damageDone);
        onHealthChange.Invoke(health);


        // If Enemy dies
        if (health == 0)
        {
            for (int i = 0; i < goldDropAmount; i++)
            {
                GameObject selectedCoin = null;

                float randomValue = UnityEngine.Random.Range(0f, 1f);

                    if (randomValue < 0.2f) // 10% chance of gold
                    {
                        selectedCoin = GoldCoinPrefab;
                    }
                    else if (randomValue >= 0.2f) // 90% chance of silver
                    {
                        selectedCoin = SilverCoinPrefab; 
                    }

                if (selectedCoin != null)
                {
                    Vector3 randomOffset = UnityEngine.Random.insideUnitSphere * 2.0f;
                    Vector3 spawnPosition = transform.position + randomOffset;
                    Instantiate(selectedCoin, spawnPosition, Quaternion.identity);
                }
            }

            Destroy(gameObject);  
        }
    }

    public void Heal(float amount)
    {
        health = (int) Math.Min(maxHealth, health + amount);
        onHealthChange.Invoke(health);
    }
    public float GetStrength() => damage;
    public float Health() => health;
    public float MaxHealth() => maxHealth;

    void OnCollisionStay2D(Collision2D collision)
    {
        PlayerLiving player = collision.gameObject.GetComponent<PlayerLiving>();
        if (player != null) player.TakeDamage(GetStrength());
    }
}