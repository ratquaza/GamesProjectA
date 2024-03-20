using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public Rigidbody2D rb2D;
    private float movementSpeed = 200;
    private Vector2 moveDirection;
    public float playerSpeed, dashDuration, dashCooldown;

    void Start(){
        playerSpeed = movementSpeed;
    }
    void Update()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");
        moveDirection.Normalize();
        rb2D.velocity = moveDirection * playerSpeed;


        if(Input.GetKey(KeyCode.Space)){

            if(dashCooldown <= 0 && dashDuration <= 0){
                playerSpeed = movementSpeed * 4f;
                dashDuration = .2f;
                Debug.Log("dashing");
            }
        }

        if(dashDuration > 0){
            dashDuration -= Time.deltaTime;

            if(dashDuration <= 0){
                playerSpeed = movementSpeed;
                dashCooldown = 1f;
                dashDuration = 0;
            }
        }

        if(dashCooldown > 0){
            dashCooldown -= Time.deltaTime;
            if(dashCooldown <= 0){
                dashCooldown =0;
            }
        }
    }
}


