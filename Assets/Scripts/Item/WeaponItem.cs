using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 0)]
public class WeaponItem : Item
{
    [SerializeField] public GameObject weapon;

    public Weapon GetOrCreateWeapon(PlayerLiving player)
    {
        Weapon existingWeapon = player.transform.GetComponentInChildren<Weapon>();
        if (existingWeapon) return existingWeapon;
        GameObject newWeapon = Instantiate(weapon, player.transform);
        return newWeapon.GetComponent<Weapon>();
    }
}