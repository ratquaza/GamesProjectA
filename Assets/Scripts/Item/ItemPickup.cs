using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private WeaponItem itemPickup;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerLiving player = coll.GetComponent<PlayerLiving>();
        if (player == null) return;
        if (player.GiveWeapon(itemPickup))
        {
            player.EquipWeapon(itemPickup);
            Destroy(gameObject);
            return;
        }
        
        WeaponItem oldWeapon = player.GetWeaponAt(player.GetEquippedWeaponIndex());
        player.EquipWeapon(itemPickup, true);
        itemPickup = oldWeapon;
        spriteRenderer.sprite = oldWeapon.Icon;
    }
}
