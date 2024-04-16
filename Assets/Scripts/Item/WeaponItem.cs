using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 0)]
public class WeaponItem : Item
{
    [SerializeField] public GameObject weapon;

    public Weapon GetOrCreateWeapon(PlayerLiving player)
    {
        Weapon existingWeapon = player.transform.GetComponentInChildren<Weapon>();
        if (existingWeapon)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource<GameObject>(existingWeapon.gameObject);
            if (prefab != null && prefab.Equals(weapon)) return existingWeapon;
        }
        GameObject newWeapon = Instantiate(weapon, player.transform);
        return newWeapon.GetComponent<Weapon>();
    }
}