using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySet", menuName = "Enemy/EnemySet")]
public class EnemySet : ScriptableObject
{
    [SerializeField] private WeightedSpawn[] spawns;
    private float totalWeight = -1;

    void OnEnable()
    {
        totalWeight = spawns.Aggregate(0f, (value, spawn) => value + spawn.weight);
    }

    public WeightedSpawn GetSpawn()
    {
        float selectedValue = UnityEngine.Random.Range(0f, totalWeight);
        foreach (WeightedSpawn spawn in spawns)
        {
            selectedValue -= spawn.weight;
            if (selectedValue <= 0) return spawn;
        }
        return spawns[spawns.Length - 1];
    }

    [Serializable]
    public struct WeightedSpawn
    {
        public float weight;
        public Enemy enemyPrefab;
    }
}