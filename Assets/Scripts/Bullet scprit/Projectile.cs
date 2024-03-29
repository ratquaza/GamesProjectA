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
        // Save the spawn cordinate of the projectile
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > ProjectileLife)
        {
            Destroy(this.gameObject);
            //destroy projectile if Projectile Life greater than timer
        }
        //if not keep counting and move projectile
        timer += Time.deltaTime;
        transform.position = ProjectileMovement(timer);

    }

    private Vector2 ProjectileMovement(float timer)
    {
        float x = timer * ProjectileSpeed * transform.right.x;
        float y = timer * ProjectileSpeed * transform.right.y;
        // transform bullet x y to the right
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
        // then return the position of the projectile using spawn point 
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
