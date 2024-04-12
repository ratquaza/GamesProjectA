using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BreakerSword : SimpleWeapon
{
    public BreakerSword() : base(1f, 1f)
    {
    }

    protected void OnPrimaryAnimFrame()
    {
        Vector2 kbAngle = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position))).normalized * 15f;
        Weapon.DamageInCollider(primaryCollider, 3, kbAngle);
    }

    protected void OnSecondaryAnimJab()
    {
        Vector2 kbAngle = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position))).normalized * 5f;
        Weapon.DamageInCollider(secondaryCollider, 2, kbAngle);
    }

    protected void OnSecondaryAnimExpand()
    {
        Vector2 kbAngle = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint(player.transform.position))).normalized * 20f;
        Weapon.DamageInCollider(secondaryCollider, 3, kbAngle);
    }
}
