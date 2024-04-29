using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShootBehaviour", menuName = "Projectile/ShootBehaviour", order = 1)]
public class ShootBehaviour : ScriptableObject
{
    [SerializeField] public SpawnerType spawnBehaviour;
    [SerializeField] public float delay;
    [SerializeField] public int shootCount;

    // For SpawnerType.TowardsPlayer & Spinning - determines delay between each projectile if shootCount > 1
    [SerializeField] public float delayBetweenShoots = 0.5f;
    // For SpawnerType.Spinning - determines speeed that the starting position of projectile spins at
    [SerializeField] public float spinSpeed = 5;
    // For SpawnerType.Arc - determines the area that the arc shoots
    [SerializeField] public float arcDegrees = 30;
}

public enum SpawnerType
{
    TowardsPlayer,
    Spinning,
    Arc
}