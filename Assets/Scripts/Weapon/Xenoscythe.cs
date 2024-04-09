using UnityEngine;
using UnityEngine.InputSystem;

public class Xenoscythe : SimpleWeapon
{
    public Xenoscythe() : base(1f, 0.3f)
    {
        
    }

    void PrimaryEvent()
    {
        Vector2 kbAngle = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized * 20f;
        DamageInCollider(primaryCollider, 3, kbAngle);
    }

    void SecondaryEvent()
    {
        DamageInCollider(secondaryCollider, 2, (enemy) => (enemy.transform.position - transform.position).normalized * 120f);
    }
}
