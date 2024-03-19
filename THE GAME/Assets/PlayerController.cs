using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const int DASH_COOLDOWN = 2;

    [SerializeField] private Rigidbody2D Rigidbody;
    private float dashCooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            dashCooldown = DASH_COOLDOWN;
            Rigidbody.velocity += new Vector2(x, y) * 3000;
        }
    }

    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Rigidbody.velocity += new Vector2(x, y) * 50;
        Rigidbody.velocity = Vector2.ClampMagnitude(Rigidbody.velocity, 200);
        if (Math.Abs(x + y) <= 0.3) Rigidbody.velocity *= .8f;
    }
}
