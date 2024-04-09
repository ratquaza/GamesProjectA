using UnityEngine;
using UnityEngine.InputSystem;

public class Xenoscythe : Weapon
{
    [SerializeField] private BoxCollider2D primaryCollider;
    [SerializeField] private BoxCollider2D secondaryCollider;
    [SerializeField] private Animator animator;

    private readonly float PRIMARY_CD = 1f;
    private readonly float SECONDARY_CD = 0.3f;

    private float primaryCooldown = 0f;
    private float secondaryCooldown = 0f;

    private bool CanAttack(float cd)
    {
        return cd <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public override void OnEquip(PlayerLiving player, WeaponItem item, InputAction primaryInput, InputAction secondaryInput)
    {
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
    }

    void AttemptSecondary(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(secondaryCooldown)) return;
        secondaryCooldown = SECONDARY_CD;
        animator.SetTrigger("Secondary");
    }

    void PrimaryEvent()
    {
        Vector2 kbAngle = (Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position)).normalized * 20f;
        DamageInCollider(primaryCollider, 3, kbAngle);
    }

    void SecondaryEvent()
    {
        DamageInCollider(secondaryCollider, 5, (enemy) => (enemy.transform.position - transform.position).normalized * 90f);
    }
}
