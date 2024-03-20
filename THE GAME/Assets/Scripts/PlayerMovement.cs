using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dampening = 0.85f;
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
        if (currentDashDuration <= 0f) {
            rb2d.velocity += WASDInput.ReadValue<Vector2>() * moveSpeed;
            rb2d.velocity *= dampening;
            if (currentDashCooldown > 0) currentDashCooldown -= Time.deltaTime;
        } else {
            rb2d.velocity = dashDirection * dashSpeed;
            currentDashDuration -= Time.deltaTime;
            if (currentDashDuration <= 0) {
                currentDashCooldown = dashCooldown;
            }
        }
    }

    void Dash()
    {
        if (currentDashCooldown > 0) return;
        dashDirection = WASDInput.ReadValue<Vector2>();
        currentDashDuration = dashDuration;
    }
}
