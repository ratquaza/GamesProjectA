using System.Collections.Generic;
using UnityEngine;

public class ManaBubbleProjectile : MonoBehaviour
{
    [SerializeField] protected CircleCollider2D aoe;
    [SerializeField] protected float readyTime = 1f;
    [SerializeField] protected float popTime = 0.2f;

    protected float currentReadyTime = 0f;
    protected float currentPopTime = 0f;
    protected bool isReady = false;
    protected bool isPopping = false;
    protected Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        transform.localScale *= .1f;
        aoe.enabled = false;
        currentReadyTime = readyTime;
        currentPopTime = popTime;
    }

    void Update()
    {
        if (!isReady && currentReadyTime >= 0)
        {
            currentReadyTime -= Time.deltaTime;
            transform.localScale = Vector3.Lerp(initialScale, transform.localScale, currentReadyTime/readyTime);
            if (currentReadyTime <= 0) OnReady();
        }
        if (isReady && isPopping && currentPopTime >= 0)
        {
            currentPopTime -= Time.deltaTime;
            if (currentPopTime <= 0) OnPop();
        }
    }

    protected void OnReady()
    {
        isReady = true;
        aoe.enabled = true;
    }

    protected void OnPop()
    {
        List<Collider2D> hits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        aoe.OverlapCollider(filter, hits);
        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                Rigidbody2D rb2d = hit.GetComponent<Rigidbody2D>();
                enemy.TakeDamage(3);
                rb2d.velocity += ((Vector2) (enemy.transform.position - transform.position)).normalized * 50f;
            }
            ManaBubbleProjectile bubble = hit.GetComponent<ManaBubbleProjectile>();
            if (bubble != null) bubble.SetToPop();
        }
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (isReady && collision.collider.GetComponent<ManaBubbleProjectile>() == null) isPopping = true;
    }

    public void SetToPop()
    {
        isReady = true;
        isPopping = true;
    }
}
