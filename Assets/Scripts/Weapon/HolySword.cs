using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HolySword : SimpleWeapon
{
    protected PlayerMovement playerMovement;
    public HolySword() : base(0f, .5f)
    {
    }

    public override void OnEquip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput)
    {
        base.OnEquip(player, primaryInput, secondaryInput);
        playerMovement = player.GetComponent<PlayerMovement>();
    }

    protected override void OnPrimary(InputAction.CallbackContext ctx)
    {  
        Vector2 kbAngle = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position))).normalized * 15f;
        DamageInCollider(primaryCollider, 3, kbAngle);
    }

    protected override void OnSecondary(InputAction.CallbackContext ctx)
    {
        Vector2 kbAngle = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position))).normalized * 35f;
        Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;
        float velocityBonus = playerMovement.IsDashing() ? (float) Math.Floor(Math.Max(0, Vector2.Dot(velocity.normalized, kbAngle.normalized) + .2f)) : 0;
        
        // Deals +5damage if player is dashing towards poke direction
        DamageInCollider(secondaryCollider, 1 + 5 * velocityBonus, kbAngle);
    }
}
