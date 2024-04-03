using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Animator anim; 
    [SerializeField] private float attackSpeed;
    [SerializeField] private float playerDamage = 50;
    float attackCooldown; 

    private void Update() {
        if(attackCooldown <= 0){
            if(Input.GetMouseButtonDown(0)){
                anim.SetTrigger("Attack");
                attackCooldown = attackSpeed;
                
            }
        } else{
            attackCooldown -= Time.deltaTime;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy"){
            other.GetComponent<Enemy>().TakeDamage(playerDamage);
            Debug.Log("Attack works");
        }
    }

}
