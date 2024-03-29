using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float ProjectileLife = 1f;
    [SerializeField] private float ProjectileRotation = 0f;
    [SerializeField] private float ProjectileSpeed = 1f;

    private float timer = 0f;
    private Vector2 spawnPoint;
    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > ProjectileLife)
        {
            Destroy(this.gameObject);
        }
        
        timer += Time.deltaTime;
        transform.position = ProjectileMovement(timer);

    }

    private Vector2 ProjectileMovement(float timer)
    {
        float x = timer * ProjectileSpeed * transform.right.x;
        float y = timer * ProjectileSpeed * transform.right.y;
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
    }

    void doesRicochet()
    {

    }

    void OnCollisionEnter2D()
    {

    }

    void desPredictiveTrajectory()
    {

    }
}
