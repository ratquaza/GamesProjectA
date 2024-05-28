using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerTeleporter : MonoBehaviour
{
    [SerializeField] protected string sceneName;

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerLiving player = collider.GetComponentInParent<PlayerLiving>();

        if (player != null)
        {
            player.transform.position = new Vector2(5, 5);
            GameManager.Instance.ToDungeon(sceneName);
        }
    }
}
