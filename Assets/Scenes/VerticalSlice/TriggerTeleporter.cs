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
        if (player == null) return;
        GameManager.Instance.ToDungeon(sceneName);
    }
}
