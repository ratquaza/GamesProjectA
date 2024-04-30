using UnityEngine;

public class GoldCoin : MonoBehaviour
{
    private PlayerLiving player;
    [SerializeField] private int goldAmount;

    public int GoldAmount => goldAmount;

    private void Start()
    {
        player = FindObjectOfType<PlayerLiving>();
        if (player == null)
        {
            Debug.LogError("PlayerLiving component not found in the scene.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (player != null)
            {
                player.AddGold(goldAmount);
            }
            else
            {
                Debug.LogWarning("PlayerLiving component is not assigned.");
            }
            Destroy(gameObject);
        }
    }
}
