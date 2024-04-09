using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HolySword : Weapon
{
    [SerializeField] private BoxCollider2D primaryCollider;
    [SerializeField] private BoxCollider2D secondaryCollider;
    [SerializeField] private Animator animator;

    private readonly float PRIMARY_CD = 1f;
    private readonly float SECONDARY_CD = 0.3f;

    private float primaryCooldown = 0f;
    private float secondaryCooldown = 0f;

    private PlayerMovement player;

    private bool CanAttack(float cd)
    {
        return cd <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public override void OnEquip(PlayerLiving player, WeaponItem item, InputAction primaryInput, InputAction secondaryInput)
    {
        this.player = player.GetComponent<PlayerMovement>();
        primaryInput.started += AttemptPrimary;
        secondaryInput.started += AttemptSecondary;
    }

    public override void OnUnequip(PlayerLiving player, WeaponItem item, InputAction primaryInput, InputAction secondaryInput)
    {
        primaryInput.started -= AttemptPrimary;
        secondaryInput.started -= AttemptSecondary;
    }

    void Update()
    {
        if (primaryCooldown > 0) primaryCooldown -= Time.deltaTime;
        if (secondaryCooldown > 0) secondaryCooldown -= Time.deltaTime;
    }

    void AttemptPrimary(InputAction.CallbackContext ctx)
    {   
        if (!CanAttack(primaryCooldown)) return;
        primaryCooldown = PRIMARY_CD;
        animator.SetTrigger("Primary");
        Vector2 kbAngle = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized * 15f;
        DamageInCollider(primaryCollider, 3, kbAngle);
    }

    void AttemptSecondary(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(secondaryCooldown)) return;
        secondaryCooldown = SECONDARY_CD;
        animator.SetTrigger("Secondary");

        Vector2 kbAngle = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized * 35f;
        Vector2 velocity = player.GetComponent<Rigidbody2D>().velocity;
        float velocityBonus = player.IsDashing() ? (float) Math.Floor(Math.Max(0, Vector2.Dot(velocity.normalized, kbAngle.normalized) + .2f)) : 0;
        
        // Deals +5damage if player is dashing towards poke direction
        DamageInCollider(secondaryCollider, 1 + 5 * velocityBonus, kbAngle);
    }
}
