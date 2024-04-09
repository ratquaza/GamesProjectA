using UnityEngine;
using UnityEngine.InputSystem;

public abstract class SimpleWeapon : Weapon
{
    [SerializeField] protected BoxCollider2D primaryCollider;
    [SerializeField] protected BoxCollider2D secondaryCollider;
    [SerializeField] protected Animator animator;

    protected PlayerLiving player;
    protected readonly float PRIMARY_COOLDOWN;
    protected readonly float SECONDARY_COOLDOWN;

    protected float primaryCooldown = 0f;
    protected float secondaryCooldown = 0f;

    public SimpleWeapon(float primaryCooldown, float secondaryCooldown)
    {
        this.PRIMARY_COOLDOWN = primaryCooldown;
        this.SECONDARY_COOLDOWN = secondaryCooldown;
    }

    public override void OnEquip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput)
    {
        this.player = player;
        primaryInput.started += AttemptPrimary;
        secondaryInput.started += AttemptSecondary;
    }

    public override void OnUnequip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput)
    {
        primaryInput.started -= AttemptPrimary;
        secondaryInput.started -= AttemptSecondary;
    }

    protected virtual bool CanAttack(float cd)
    {
        return cd <= 0 && (animator == null || animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
    }

    void Update()
    {
        if (primaryCooldown > 0) primaryCooldown -= Time.deltaTime;
        if (secondaryCooldown > 0) secondaryCooldown -= Time.deltaTime;
    }

    void AttemptPrimary(InputAction.CallbackContext ctx)
    {   
        if (!CanAttack(primaryCooldown)) return;
        primaryCooldown = PRIMARY_COOLDOWN;
        if (animator != null) animator.SetTrigger("Primary");
        OnPrimary(ctx);
    }

    void AttemptSecondary(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(secondaryCooldown)) return;
        secondaryCooldown = SECONDARY_COOLDOWN;
        if (animator != null) animator.SetTrigger("Secondary");
        OnSecondary(ctx);
    }

    protected virtual void OnPrimary(InputAction.CallbackContext ctx) {}
    protected virtual void OnSecondary(InputAction.CallbackContext ctx) {}
}