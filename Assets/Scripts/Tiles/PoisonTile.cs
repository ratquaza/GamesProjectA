using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTile : MonoBehaviour
{
    [SerializeField] private float poisonDamage = 10;
    [SerializeField] private float poisonDelay = 1f;
    [SerializeField] private int numPoisonTicks = 3;
    private bool isPoisoning = false;


    private void Update() {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartPoison(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StopPoison(other.gameObject);
        }
    }

    private void StartPoison(GameObject target)
    {
        if (!isPoisoning)
        {
            isPoisoning = true;
            StartCoroutine(Poison(target));
        }
    }

    private void StopPoison(GameObject target)
    {
        if (isPoisoning)
        {
            isPoisoning = false;
        }
    }

    private IEnumerator Poison(GameObject target)
    {
        if (isPoisoning)
        {
            for (int i = 0; i < numPoisonTicks; i++)
            {   
                yield return new WaitForSeconds(poisonDelay);
                DealDamage(target, poisonDamage);
                Debug.Log("test");
            }
            
            yield return null;
        }
    }

    private void DealDamage(GameObject target, float damage)
    {
        PlayerLiving playerLiving = target.GetComponent<PlayerLiving>();
        if (playerLiving != null)
        {
            playerLiving.TakeDamageFinal(damage);
        }
    }
}
