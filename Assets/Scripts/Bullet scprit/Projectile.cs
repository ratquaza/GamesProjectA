using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    public float projectileLife = 1f;
    public float projectileRotation = 0f;
    public float projectileSpeed = 1f;

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
        if(timer > projectileLife)
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
        float x = timer * projectileSpeed * transform.right.x;
        float y = timer * projectileSpeed * transform.right.y;
        // transform bullet x y to the right
        return new Vector2(x + spawnPoint.x, y + spawnPoint.y);
        // then return the position of the projectile using spawn point 
    }

    void doesRicochet()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
        if (collision.gameObject == null) return;
        PlayerLiving player = collision.gameObject.GetComponent<PlayerLiving>();
        if (player == null) return;
        player.TakeDamage(20);
        // Destroy when collided 
    }

    void doesPredictiveTrajectory()
    {

    }
}
