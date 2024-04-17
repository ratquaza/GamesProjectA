using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileInfo", menuName = "Projectile/ProjectileInfo", order = 0)]
public class ProjectileInfo : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private bool ricochet;
    [SerializeField] private float damage;
    [SerializeField] private Sprite sprite;
}
