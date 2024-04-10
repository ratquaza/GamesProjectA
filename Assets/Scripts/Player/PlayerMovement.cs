using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.MPE;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float drag = 10f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private int dashSidesteps = 1;

    [Header("Movement Test Options")]
    [Tooltip("When the player sidesteps during the dash, should the current dash duration be extended to 80% of dash duration.")]
    [SerializeField] private bool resetDashOnSidestep = false;
    [Tooltip("During the dash, should the velocity have an 'oomph' to it - where the velocity is higher at the start then regular speed at the end.")]
    [SerializeField] private bool dashOomph = false;
    [Tooltip("When a dash ends but the player is still moving, give them a grace on their maxspeed. Fixes the hard stopping after dash.")]
    [SerializeField] private bool graceAfterDash = false;

    private PlayerActions playerActions;
    private InputAction walkInput;
    private InputAction dashInput;
    private Rigidbody2D rb2d;
    private Vector2 lookDirection = Vector2.down; 
    private float currentDashDuration = 0f;
    private float currentDashCooldown = 0f;
    private int currentDashSidesteps ;
    private float dashGrace = .2f;
    private float currentDashGrace;

    

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerActions = new PlayerActions();

        walkInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;

        dashInput.performed += ctx => AttemptDash();
        currentDashSidesteps = dashSidesteps;
        currentDashGrace = dashGrace;
        rb2d.drag = drag;
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
        Vector2 inputDir = walkInput.ReadValue<Vector2>();
        
        if (IsDashing()) HandleDash(inputDir); 
        else
        {
            if (inputDir.magnitude > 0) HandleInput(inputDir);
            if (currentDashCooldown > 0) currentDashCooldown -= Time.deltaTime;
            if (graceAfterDash && currentDashGrace > 0) currentDashGrace -= Time.deltaTime;
        }
    }

    public bool IsDashing()
    {
        return currentDashDuration > 0f;
    }

    void AttemptDash()
    {
        // If cooldown hasn't ended, return
        if (currentDashCooldown > 0) return;
        // Set the dash duration and total sidesteps
        currentDashDuration = dashDuration;
        currentDashSidesteps = dashSidesteps;
    }

    public Vector2 GetLookDirection(){
        return lookDirection;
    }

    void HandleDash(Vector2 input)
    {
        // Check if the player is inputting, and if they have enough sidesteps
        if (currentDashSidesteps > 0 && input.magnitude > 0)
        {
            float dot = Vector2.Dot(input, lookDirection);
            // If the player's input is more than 10% diff to the original dash
            if (dot < 0.8)
            {
                // Reduce their total sidesteps, and cange the direction
                currentDashSidesteps--;
                lookDirection = input.normalized;
                if (resetDashOnSidestep) currentDashDuration = dashDuration * .5f;
            }
        }
        // Force them to move towards their last direction at dashSpeed
        // Additional math that adds a little bit of an oomf at the start
        rb2d.velocity = lookDirection * dashSpeed;
        if (dashOomph) rb2d.velocity *= 1f + 2f * (currentDashDuration/dashDuration);
        currentDashDuration -= Time.deltaTime;
        // When player's dash ends, begin cooldown
        if (currentDashDuration <= 0) {
            currentDashCooldown = dashCooldown;
            if (graceAfterDash) currentDashGrace = dashGrace;
        }
    }

    void HandleInput(Vector2 input)
    {
        // Add player's input to the velocity and clamp it
        rb2d.velocity += input * moveSpeed;
        if (input.magnitude > 0) lookDirection = input.normalized;
        float relativeMaxSpeed = graceAfterDash ? Math.Max(dashSpeed * (currentDashGrace/dashGrace), maxSpeed) : maxSpeed;
        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, relativeMaxSpeed);
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public void SetMaxSpeed(float amount)
    {
        this.maxSpeed = Math.Max(1, amount);
    }
}
