using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [SerializeField] private WeaponItem itemPickup;

    void OnTriggerEnter2D(Collider2D coll)
    {
        PlayerLiving player = coll.GetComponent<PlayerLiving>();
        if (player == null) return;
        player.EquipWeapon(itemPickup);
        Destroy(gameObject);
    }
}
