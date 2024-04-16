using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    enum BulletType
    {
        Straight,
        Aiming
    }
    public float projectileLife = 1f;
    public float projectileRotation = 0f;
    public float projectileSpeed = 1f;
    [SerializeField] private BulletType bulletType;

    private float timer = 0f;
    private Vector2 spawnPoint;
    private GameObject player;
    private Rigidbody2D rb;

    void Start()
    {
        spawnPoint = new Vector2(transform.position.x, transform.position.y);
        // Save the spawn cordinate of the projectile
        
        if(bulletType == BulletType.Aiming)
        {
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindGameObjectWithTag("Player");

           ProjectileAiming();
        }
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
        
        if (bulletType == BulletType.Straight)
        {
            transform.position = ProjectileStraight(timer);
        }
    }


    void ProjectileAiming()
    {
     
        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

        float rotation= Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + 90);
        
    }

    private Vector2 ProjectileStraight(float timer)
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
        // Destroy when collided 
    }

    void desPredictiveTrajectory()
    {

    }
}
