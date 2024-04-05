using UnityEngine;

public class WeaponHandler : MonoBehaviour
{   
	[SerializeField] private SpriteRenderer heldSprite;
    [SerializeField] private BoxCollider2D primaryCollider;
    [SerializeField] private BoxCollider2D secondaryCollider;
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

		heldSprite.flipY = transform.rotation.z < -.6 || transform.rotation.z > .6;
    }

    public void UpdateWeapon(WeaponItem item)
    {
        item.DefineColliders(primaryCollider, secondaryCollider);
        this.currentEquipped = item;
    }

    public WeaponItem GetWeapon()
    {
        return this.currentEquipped;
    }

    public void PrimaryAttack(PlayerLiving player)
    {
        primaryCollider.enabled = true;
        currentEquipped.PrimaryAttack(player, primaryCollider);
        primaryCollider.enabled = false;
    }

    public void SecondaryAttack(PlayerLiving player)
    {
        secondaryCollider.enabled = true;
        currentEquipped.SecondaryAttack(player, secondaryCollider);
        secondaryCollider.enabled = false;
    }
}
