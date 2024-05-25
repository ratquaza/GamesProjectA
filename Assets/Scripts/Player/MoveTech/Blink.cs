using UnityEngine;
using UnityEngine.InputSystem;

public class Blink : MonoBehaviour
{
    [SerializeField] private GameObject blinkTarget;
    [SerializeField] private Rigidbody2D blinkTargetRb;
    [SerializeField] private BlinkTarget blinkTargetScript;

    [SerializeField] private float moveSpeedMultiplier = 2f; // Blink target movement speed
    [SerializeField] private float blinkDistance  = 0f; // Probably leave as zero to prevent phasing

    private PlayerMovement playerMovement;
    private PlayerActions playerActions;
    private bool isBlinking = false;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        blinkTargetRb = blinkTarget.GetComponent<Rigidbody2D>();
        blinkTargetScript = blinkTarget.GetComponent<BlinkTarget>();

        playerActions = new PlayerActions();
        playerActions.Movement.Blink.performed += ctx => OnBlinkStarted();
        playerActions.Movement.Blink.canceled += ctx => OnBlinkCanceled();
    }

    private void Update()
    {
        Debug.Log(blinkTargetScript.GetCanTeleport()); 

        if (isBlinking)
        {
            Vector2 movementInput = playerActions.Movement.Walk.ReadValue<Vector2>();
            Vector2 movementDirection = movementInput.normalized;
            blinkTargetRb.velocity = movementDirection * moveSpeedMultiplier;
        }
    }

    private void OnBlinkStarted()
    {
        Vector2 lookDirection = playerMovement.GetLookDirection().normalized * blinkDistance;
        blinkTarget.transform.position = transform.position + new Vector3(lookDirection.x, lookDirection.y, 0f);
        playerMovement.enabled = false;
        isBlinking = true;
        blinkTarget.SetActive(true);
    }

    private void OnBlinkCanceled()
    {
        // Teleport the player directly onto the blink target
        if (blinkTargetScript.GetCanTeleport())
        {
            transform.position = blinkTarget.transform.position;
        }
        else
        {
            transform.position = blinkTargetScript.GetLastValidLocation();
        }

        playerMovement.enabled = true;
        isBlinking = false;
        blinkTarget.SetActive(false);
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
}
