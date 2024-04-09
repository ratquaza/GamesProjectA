using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Blink : MonoBehaviour
{
    [SerializeField] private float blinkDistance = 5f; //distance to blink
    [SerializeField] private float blinkCooldown = 1f; 
    [SerializeField] private float blinkDelay = 0.5f; //delay before teleporting player
    [SerializeField] private float scaleMultiplier = 1.6f;

    private PlayerMovement playerMovement;
    private PlayerActions playerActions;
    private bool canBlink = true;
    private Coroutine blinkCooldownCoroutine;
    private Vector3 originalScale;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerActions = new PlayerActions();

        playerActions.Movement.Blink.performed += _ => StartCoroutine(BlinkCoroutine());
        
        originalScale = transform.localScale;
    }

    IEnumerator BlinkCoroutine()
    {
        if (canBlink)
        {
            canBlink = false;

            // Scale up the player gradually during the delay time
            float elapsedTime = 0f;
            while (elapsedTime < blinkDelay)
            {
                float t = elapsedTime / blinkDelay;
                transform.localScale = Vector3.Lerp(originalScale, originalScale * scaleMultiplier, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //set scale to target scale after delay
            transform.localScale = originalScale * scaleMultiplier;

            Vector3 blinkDirection = playerMovement.GetLookDirection(); //Get look direction from PlayerMovement script
            Vector3 blinkPosition = transform.position + blinkDirection.normalized * blinkDistance; //calculate destination position

            //teleport to destination
            transform.position = blinkPosition;
            transform.localScale = originalScale;
            

            //start cooldown
            if (blinkCooldownCoroutine == null)
            {
                blinkCooldownCoroutine = StartCoroutine(BlinkCooldown());
            }
        }
    }

    IEnumerator BlinkCooldown()
    {
        yield return new WaitForSeconds(blinkCooldown);
        canBlink = true;
        blinkCooldownCoroutine = null;
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    void OnDisable()
    {
        //unsubscribe from the Blink action when script destroyed
        playerActions.Movement.Blink.performed -= _ => StartCoroutine(BlinkCoroutine());
        playerActions.Disable();
    }
}