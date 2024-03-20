using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private PlayerActions playerActions;
    private InputAction WASDInput;
    private InputAction dashInput;
    private Rigidbody2D body;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        playerActions = new PlayerActions();

        WASDInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;
        playerActions.Movement.Enable();

        WASDInput.performed += ctx => body.velocity += ctx.ReadValue<Vector2>() * moveSpeed;
    
        //call Dash() on 'Space'
        dashInput.performed += ctx => Dash();
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    void Dash()
    {
        Debug.Log("dash");
        //TODO: Complete Dash Implementation
    }
}
