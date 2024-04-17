using System.Collections;
using UnityEngine;

public class VoidTileFall : MonoBehaviour
{
    private VoidTile voidTile;
    private PlayerLiving player;
    private PlayerMovement playerMovement;

    [Header("Animation Speed")]
    [SerializeField] private float rotationSpeed = 180f;
    [SerializeField] private float scaleSpeed = 0.01f;

    [Header("Damage Taken")]
    [SerializeField] private int damageTaken = 20;

    private Transform playerSpriteTransform;

    private void Start()
    {
        voidTile = GetComponentInParent<VoidTile>(); 
        player = FindObjectOfType<PlayerLiving>();
        playerMovement = player.GetComponent<PlayerMovement>();
        var spriteRenderer = player.GetComponentInChildren<SpriteRenderer>(); 
        playerSpriteTransform = spriteRenderer.transform; 
    }

    private float fallAnimationDuration = 2f;

    private IEnumerator TeleportPlayerDelayed(Vector3 position)
    {
        playerMovement.enabled = false;

        Quaternion initialRotation = playerSpriteTransform.localRotation; 
        Vector3 initialScale = player.transform.localScale;

        float elapsedTime = 0f;
        while (elapsedTime < fallAnimationDuration) 
        {
            
            playerSpriteTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);

            
            player.transform.localScale -= Vector3.one * scaleSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerSpriteTransform.localRotation = initialRotation; 
        player.transform.position = position;
        player.transform.localScale = initialScale;
        player.TakeDamage(damageTaken);

        playerMovement.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            player.SetIFrames(player.GetIFrames());
            player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            Vector3 previousPosition = voidTile.GetPreviousLocation();
            
            StartCoroutine(TeleportPlayerDelayed(previousPosition));
        }   
    }
}
