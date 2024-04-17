using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileShootInfo", menuName = "Projectile/ProjectileShootInfo", order = 0)]
public class ProjectileShootInfo : ScriptableObject
{
    [SerializeField] public ShootStyle shootStyle;
    [SerializeField] public float shootDelay;
    [SerializeField] public int shootCount;

    public enum ShootStyle
    {
        Straight, Arc, Spin
    }
}
