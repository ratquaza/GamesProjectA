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

    private PlayerActions playerActions;
    private InputAction WASDInput;
    private InputAction dashInput;
    private Rigidbody2D rb2d;

    private float currentDashDuration = 0f;
    private float currentDashCooldown = 0f;
    private Vector2 dashDirection = Vector2.zero;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerActions = new PlayerActions();

        WASDInput = playerActions.Movement.Walk;
        dashInput = playerActions.Movement.Dash;

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

    void Update()
    {
        Vector2 inputDir = WASDInput.ReadValue<Vector2>();
        // If player is not dashing
        if (currentDashDuration <= 0f) 
        {
            // If player is not inputting movement
            if (inputDir.magnitude == 0) 
            {
                // Begin decceleration/dampening
                rb2d.velocity *= dampening;
            } else
            {
                // Add player's input to the velocity and clamp it
                rb2d.velocity += inputDir * moveSpeed;
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }
            if (currentDashCooldown > 0) currentDashCooldown -= Time.deltaTime;
        } else 
        // If player is dashing
        {
            // Keep them dashing in the dashDirection at dashSpped
            rb2d.velocity = dashDirection * dashSpeed;
            currentDashDuration -= Time.deltaTime;
            // When player's dash ends, begin cooldown
            if (currentDashDuration <= 0) {
                currentDashCooldown = dashCooldown;
            }
        }
    }

    void Dash()
    {
        // If cooldown hasn't ended, return
        if (currentDashCooldown > 0) return;
        // Get the player's current input and set the dash direction to that
        dashDirection = WASDInput.ReadValue<Vector2>();
        currentDashDuration = dashDuration;
    }
}
