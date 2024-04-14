using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private SpriteRenderer hpBar;
    [SerializeField] private SpriteRenderer hpBgBar;
    [SerializeField] private SpriteRenderer hpHeart;
    private float visibleTime = 0f;

    void Start()
    {
        hpBar.enabled = false;
        hpBgBar.enabled = false;
        hpHeart.enabled = false;
        enemy.onHealthChange += (hp) => {
            hpBar.transform.localScale = new Vector3((float) hp/enemy.MaxHealth(), 1, 1);
            visibleTime = 2f;
            hpBar.enabled = true;
            hpBgBar.enabled = true;
            hpHeart.enabled = true;
        };
    }

    void Update()
    {
        if (visibleTime > 0)
        {
            visibleTime -= Time.deltaTime;
            if (visibleTime <= 0)
            {
                hpBar.enabled = false;
                hpBgBar.enabled = false;
                hpHeart.enabled = false;
            }
        }
    }
}
