using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BluesteelLance : SimpleWeapon
{
    [SerializeField] private FollowMouse followMouseScript;
    private Rigidbody2D playerRB;
    private Vector2 drillDirection;
    private bool isDrilling;

    public override void OnEquip(PlayerLiving player, InputAction primaryInput, InputAction secondaryInput)
    {
        base.OnEquip(player, primaryInput, secondaryInput);
        playerRB = player.GetComponent<Rigidbody2D>();
    }

    void BeginDrill()
    {
        drillDirection = ((Vector2) (Input.mousePosition - Camera.main.WorldToScreenPoint (player.transform.position))).normalized;
        isDrilling = true;
        followMouseScript.enabled = false;
    }

    void StopDrill()
    {
        isDrilling = false;
        followMouseScript.enabled = true;
    }

    protected override void Update()
    {
        base.Update();
        if (isDrilling) playerRB.velocity += drillDirection;
    }
}
