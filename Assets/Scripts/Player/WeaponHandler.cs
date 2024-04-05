using UnityEngine;

public class WeaponHandler : MonoBehaviour
{   
    [SerializeField] private BoxCollider2D primaryCollider;
    [SerializeField] private BoxCollider2D secondaryCollider;
    [SerializeField] private Animator animator;
    private float primaryAttackCooldown = 0f;
    private float secondaryAttackCooldown = 0f;
    private WeaponItem currentEquipped;
	
    void Start()
    {
        UpdateWeapon(new HolySwordItem());
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (primaryAttackCooldown > 0) primaryAttackCooldown -= Time.deltaTime;
        if (secondaryAttackCooldown > 0) secondaryAttackCooldown -= Time.deltaTime;
    }

    public void UpdateWeapon(WeaponItem item)
    {
        item.DefineColliders(primaryCollider, secondaryCollider);
        currentEquipped = item;
    }

    public WeaponItem GetWeapon()
    {
        return currentEquipped;
    }

    private bool CanAttack(float cd)
    {
        return cd <= 0 && animator.GetCurrentAnimatorStateInfo(0).IsName("Idle");
    }

    public bool PrimaryAttack(PlayerLiving player)
    {
        if (!CanAttack(primaryAttackCooldown)) return false;
        animator.SetTrigger("PrimaryAttack");
        currentEquipped.PrimaryAttack(player, primaryCollider, this);
        primaryAttackCooldown = currentEquipped.primaryCooldown;
        return true;
    }

    public bool SecondaryAttack(PlayerLiving player)
    {
        if (!CanAttack(secondaryAttackCooldown)) return false;
        animator.SetTrigger("SecondaryAttack");
        currentEquipped.SecondaryAttack(player, secondaryCollider, this);
        secondaryAttackCooldown = currentEquipped.secondaryCooldown;
        return true;
    }
}
