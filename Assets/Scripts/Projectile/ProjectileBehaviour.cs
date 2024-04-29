using UnityEngine;

[CreateAssetMenu(fileName = "Projectile", menuName = "Projectile/ProjectileBehaviour", order = 0)]
public class ProjectileBehaviour : ScriptableObject
{
    [SerializeField] public MoveBehaviour moveBehaviour = MoveBehaviour.Forward;
    [SerializeField] public Sprite projectileSprite;
    [SerializeField] public float lifetime = 10;
    [SerializeField] public float speed = 1;
    [SerializeField] public float size = 1;
    [SerializeField] public float damage = 1;
    [SerializeField] public bool destroyOnHittingWall = true;
}

public enum MoveBehaviour
{
    Forward, Homing
}