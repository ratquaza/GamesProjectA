using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Items/Weapon", order = 0)]
public class WeaponItem : Item
{
    [SerializeField] public GameObject weapon;
    [SerializeField] private WeaponType weaponType;

    public Weapon GetOrCreateWeapon(PlayerLiving player)
    {
        Weapon existingWeapon = player.transform.GetComponentInChildren<Weapon>();
        if (existingWeapon)
        {
           
        }
        GameObject newWeapon = Instantiate(weapon, player.transform);
        return newWeapon.GetComponent<Weapon>();
    }

    public Color GetWeaponTypeColor()
    {
        switch (weaponType)
        {
            case WeaponType.Melee:
                return Color.blue;
            case WeaponType.Magic:
                return new Color(0.5f, 0f, 0.5f); // Purple
            case WeaponType.Specialist:
                return new Color(1f, 0.5f, 0.5f); // Pink
            case WeaponType.Meme:
                return new Color(1f, 0.5f, 0f); // Orange
            default:
                return Color.gray;
        }
    }

    public string GetWeaponType() => weaponType.ToString();
}

public enum WeaponType
{
    Melee,
    Magic,
    Specialist,
    Meme,
}