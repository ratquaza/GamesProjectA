using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemySet", menuName = "Enemy/EnemySpawnTable")]
public class EnemySpawnTable : ScriptableObject
{
    [SerializeField] public LocatedSet[] sets;

    [Serializable]
    public struct LocatedSet
    {
        public Vector2 position;
        public EnemySet set;
    }
}