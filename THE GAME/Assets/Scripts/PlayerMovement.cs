using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // Movement

    //WASD
    [SerializeField] private float moveSpeed = 4f;
    private Vector2 moveDirection;

    //Dash
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    
    // Unity Input System
    private PlayerActions playerActions;
    private InputAction WASDInput;
    private InputAction dashInput;

    void Awake()
    {
        playerActions = new PlayerActions();
        WASDInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;

        // Input Bindings

        //whenever a WASDInput is performed, update the moveDirection
        WASDInput.performed += ctx => moveDirection = ctx.ReadValue<Vector2>(); 
        WASDInput.canceled += ctx => moveDirection = Vector2.zero;
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

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        transform.Translate(moveDirection * moveSpeed);
    }

    void Dash()
    {
        Debug.Log("dash");

        //TODO: Complete Dash Implementation
    }
}
