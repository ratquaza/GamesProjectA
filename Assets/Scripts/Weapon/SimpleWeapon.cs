using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SimpleWeapon : Weapon
{
    [Header("Animations")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected string primaryTrigger;
    [SerializeField] protected string secondaryTrigger;
    [SerializeField] protected float primaryCooldown;
    [SerializeField] protected float secondaryCooldown;

    [Header("Hitbox List")]
    [SerializeField] protected Hitbox[] hitboxes;

    protected PlayerLiving player;

    protected float currPrimaryCooldown = 0f;
    protected float currSecondaryCooldown = 0f;

    public SimpleWeapon()
    {
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

    protected virtual void Update()
    {
        if (currPrimaryCooldown > 0) currPrimaryCooldown -= Time.deltaTime;
        if (currSecondaryCooldown > 0) currSecondaryCooldown -= Time.deltaTime;
    }

    protected virtual void AttemptPrimary(InputAction.CallbackContext ctx)
    {   
        if (!CanAttack(currPrimaryCooldown)) return;
        currPrimaryCooldown = primaryCooldown;
        if (animator != null) animator.SetTrigger(primaryTrigger);
    }

    protected virtual void AttemptSecondary(InputAction.CallbackContext ctx)
    {
        if (!CanAttack(currSecondaryCooldown)) return;
        currSecondaryCooldown = secondaryCooldown;
        if (animator != null) animator.SetTrigger(secondaryTrigger);
    }

    protected virtual void PerformHitbox(int hitboxID)
    {
        Hitbox hitbox = hitboxes[hitboxID];
        hitbox.box.enabled = true;
        DamageInCollider(hitbox.box, hitbox.damage, (enemy) =>
        {
            Vector2 angle = GetDirection(enemy, hitbox.box, hitbox.knockbackType);
            return angle * hitbox.knockback;
        });
        hitbox.box.enabled = false;
    }

    protected virtual Vector2 GetDirection(Enemy enemy, Collider2D collider, KnockbackType type)
    {
        Vector2 from = Vector2.zero;
        Vector2 to = Vector2.zero;

        switch (type)
        {
            case KnockbackType.AwayFromPlayer:
                from = player.transform.position;
                to = enemy.transform.position;
                break;
            case KnockbackType.AwayFromHitbox:
                from = (Vector2) collider.transform.position + collider.offset;
                to = enemy.transform.position;
                break;
            case KnockbackType.AwayFromMouse:
                from = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                to = enemy.transform.position;
                break;
            case KnockbackType.ToPlayer:
                to = player.transform.position;
                from = enemy.transform.position;
                break;
            case KnockbackType.ToHitbox:
                to = (Vector2) collider.transform.position + collider.offset;
                from = enemy.transform.position;
                break;
            case KnockbackType.ToMouse:
                to = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
                from = enemy.transform.position;
                break;
        }

        return (to - from).normalized;
    }

    [System.Serializable]
    public struct Hitbox
    {
        public Collider2D box;
        public KnockbackType knockbackType;
        public float knockback;
        public int damage;

        public Hitbox(Collider2D box, KnockbackType knockbackType, float knockback, int damage)
        {
            this.box = box;
            this.knockbackType = knockbackType;
            this.knockback = knockback;
            this.damage =  damage;
        }
    }

    public enum KnockbackType
    {
        AwayFromPlayer,
        AwayFromHitbox,
        AwayFromMouse,
        ToPlayer,
        ToHitbox,
        ToMouse,
    }
}