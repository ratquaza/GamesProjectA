using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blink : MonoBehaviour
{
    [SerializeField] private float blinkDistance = 5f; //distance to blink
    [SerializeField] private float blinkCooldown = 1f; 
    [SerializeField] private float blinkDelay = 0.5f; //delay before teleporting player
    private float scaleMultiplier = 1.2f;

    private float currentBlinkCooldown = 0f;
    private float currentBlinkTime = 0f;

    private PlayerMovement playerMovement;
    private PlayerActions playerActions;
    private Vector3 originalScale;
    private float originalMaxSpeed;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerActions = new PlayerActions();
        originalScale = transform.localScale;
        originalMaxSpeed = playerMovement.GetMaxSpeed();

        currentBlinkCooldown = blinkCooldown;
        playerActions.Movement.Blink.performed += AttemptBlink;
    }

    void Update()
    {
        if (currentBlinkCooldown > 0) currentBlinkCooldown -= Time.deltaTime;
        else if (currentBlinkTime > 0)
        {
            currentBlinkTime -= Time.deltaTime;
            OnBlinking();
            if (currentBlinkTime <= 0) OnBlinkEnd();
        }
    }

    void AttemptBlink(InputAction.CallbackContext ctx)
    {
        if (currentBlinkCooldown > 0) return;
        currentBlinkTime = blinkDelay;
    }

    void OnBlinking()
    {
        transform.localScale = Vector3.Lerp(originalScale * scaleMultiplier, originalScale, currentBlinkTime/blinkDelay);
        playerMovement.SetMaxSpeed(originalMaxSpeed * (currentBlinkTime/blinkDelay));
    }

    void OnBlinkEnd()
    {
        transform.localScale = originalScale;
        Vector3 blinkDirection = playerMovement.GetLookDirection(); //Get look direction from PlayerMovement script
        Vector3 blinkPosition = transform.position + blinkDirection.normalized * blinkDistance; //calculate destination position

        //teleport to destination
        transform.position = blinkPosition;
        currentBlinkCooldown = blinkCooldown;
        playerMovement.SetMaxSpeed(originalMaxSpeed);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        playerActions.Disable();
        playerActions.Movement.Dash.performed -= AttemptBlink;
    }
}