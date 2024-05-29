using UnityEngine;

public class InactiveRoom : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer; // Layer mask to specify the enemy layer

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            SetEnemiesActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.CompareTag("Player")) 
        {
            SetEnemiesActive(false);
        }
    }

    private void SetEnemiesActive(bool isActive)
    {
        foreach (Transform child in transform)
        {
            if (((1 << child.gameObject.layer) & enemyLayer) != 0)
            {
                child.gameObject.SetActive(isActive);
            }
        }

        Debug.Log("Enemies set to " + (isActive ? "active" : "inactive"));
    }
}