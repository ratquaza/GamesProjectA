using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected float offset;
    protected SpriteRenderer player;

    void Start()
    {
        player = PlayerLiving.Instance.GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        spriteRenderer.sortingOrder = player.transform.position.y - offset > transform.position.y ? player.sortingOrder + 1 : player.sortingOrder - 1;
    }
}
