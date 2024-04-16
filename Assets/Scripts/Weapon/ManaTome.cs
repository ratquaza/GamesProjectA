using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManaTome : SimpleWeapon
{
    [SerializeField] protected GameObject manaBubble;
    [SerializeField] protected float primaryMaxDistance = 5f;
    private Vector2 gizmoPos;

    protected void Primary()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos = player.transform.InverseTransformPoint(mousePos);
        mousePos = Vector2.ClampMagnitude(mousePos, primaryMaxDistance);
        mousePos = player.transform.TransformPoint(mousePos);
        gizmoPos = mousePos;

        DamageInSquare(mousePos, Vector2.one * 1.5f, 1, (enemy) => ((Vector2) (enemy.transform.position - player.transform.position)).normalized * 30f);
    }

    protected void Secondary()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < 3; i++)
        {
            GameObject bubble = Instantiate(manaBubble);
            Vector2 offset = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-1.5f, 1.5f));
            bubble.transform.position = mousePos + offset;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(gizmoPos, Vector2.one * 1.5f);
    }
}
