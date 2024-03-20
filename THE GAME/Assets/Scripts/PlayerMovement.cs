using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 300f;
    [SerializeField] private float dampening = 0.5f;
    [SerializeField] private float dashSpeed = 2000f;
    [SerializeField] private float dashDuration = 0.1f;
    [SerializeField] private float dashCooldown = 0.3f;
    
    private PlayerActions playerActions;
    private InputAction WASDInput;
    private InputAction dashInput;
    private Rigidbody2D rb2d;

    private float currentDashDuration = 0f;
    private float currentDashCooldown = 0f;
    private Vector2 dashDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.up; 
    
    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerActions = new PlayerActions();

        WASDInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;
        
        //bind dash
        dashInput.performed += ctx => PerformDash();
    }

    void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
    }

    void Update()
    {
        WalkOrDashChecker();
    }

    private void WalkOrDashChecker()
    {
        //walk logic
        if (currentDashDuration <= 0f) 
        {
            Vector2 movementInput = WASDInput.ReadValue<Vector2>();
            if (movementInput.magnitude > 0) {
                lookDirection = movementInput.normalized;
            }

            rb2d.velocity += movementInput * moveSpeed;
            rb2d.velocity *= dampening;
            if (currentDashCooldown > 0)
            {
                currentDashCooldown -= Time.deltaTime;
            }
        }

        //if not walking -> dash logic
        else 
        {
            rb2d.velocity = dashDirection * dashSpeed;
            currentDashDuration -= Time.deltaTime;
            if (currentDashDuration <= 0) {
                currentDashCooldown = dashCooldown;
            }
        }
    }

    void PerformDash()
    {
        if (currentDashCooldown > 0) return;
        
        if (rb2d.velocity.magnitude == 0) 
        {
            //if player is standing still dash in the last look direction
            dashDirection = lookDirection;
        } 
        else 
        {
            //otherwise dash in the direction of movement
            dashDirection = WASDInput.ReadValue<Vector2>().normalized;
        }
        
        currentDashDuration = dashDuration;
    }
}
