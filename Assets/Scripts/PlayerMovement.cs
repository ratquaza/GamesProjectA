using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dampening = 0.85f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private Camera playerCamera;

    private PlayerActions playerActions;
    private InputAction walkInput;
    private InputAction dashInput;
    private Rigidbody2D rb2d;

    private float currentDashDuration = 0f;
    private float currentDashCooldown = 0f;
    private Vector2 dashDirection = Vector2.zero;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerActions = new PlayerActions();

        walkInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;

        dashInput.performed += ctx => AttemptDash();
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
        
        if (IsDashing()) HandleDash(); 
        else
        {
            if (inputDir.magnitude == 0) HandleNoInput();
            else HandleInput(inputDir);
            if (currentDashCooldown > 0) currentDashCooldown -= Time.deltaTime;
        }
    }

    bool IsDashing()
    {
        return currentDashDuration > 0f;
    }

    void AttemptDash()
    {
        // If cooldown hasn't ended, return
        if (currentDashCooldown > 0) return;
        // Set the dash duration
        currentDashDuration = dashDuration;
    }

    void HandleDash()
    {
        // Force them to move towards their last direction at dashSpeed
        rb2d.velocity = dashDirection * dashSpeed;
        currentDashDuration -= Time.deltaTime;
        // When player's dash ends, begin cooldown
        if (currentDashDuration <= 0) {
            currentDashCooldown = dashCooldown;
        }
    }

    void HandleNoInput()
    {
        // Begin decceleration/dampening
        rb2d.velocity *= dampening;
    }

    void HandleInput(Vector2 input)
    {
        // Add player's input to the velocity and clamp it
        rb2d.velocity += input * moveSpeed;
        dashDirection = input.normalized;
        rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
    }
}
