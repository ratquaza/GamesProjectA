using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;

public class ArcShoot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject player;
    [SerializeField] private float bulletMoveSpeed = 5f;
    [SerializeField] private int burstCount;
    [SerializeField] private int projectilesPerBurst;
    [SerializeField] [Range(0, 359)] private float angleSpread; 
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts;
    [SerializeField] private float restTime = 1f;
    
//
    
//

    private bool isShooting = false;
    
    public void Attack(){
        if(!isShooting){
            StartCoroutine(ShootRoutine());
        }
    }
    private IEnumerator ShootRoutine() {
        isShooting = true;
    
    Vector2 targetDirection = player.transform.position - transform.position;
    float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
    float startAngle = targetAngle;
    float endAngle = targetAngle;
    float currentAngle = targetAngle;
    float halfAnglSpread = 0f;
    float angleStep = 0;




    if(angleSpread != 0){
        angleStep = angleSpread / (projectilesPerBurst - 1);
        halfAnglSpread = angleSpread / 2f;
        startAngle = targetAngle - halfAnglSpread;
        endAngle = targetAngle + halfAnglSpread;
        currentAngle = startAngle;
    }

    for(int i = 0; i < burstCount; i++){
        for(int j = 0; j < projectilesPerBurst; j++){
        Vector2 pos = FindBulletSpawnPos(currentAngle);
        GameObject newBullet = Instantiate(projectile, pos, quaternion.identity);
        newBullet.transform.right = newBullet.transform.position - transform.position;
        if(newBullet.TryGetComponent(out Projectile bullet)){
            bullet.UpdateMoveSpeed(bulletMoveSpeed);
        }
        currentAngle += angleStep;

        }

        currentAngle = startAngle;
        yield return new WaitForSeconds(timeBetweenBursts);
    }
    yield return new WaitForSeconds(restTime);
    isShooting = false;

    }
    private Vector2 FindBulletSpawnPos(float currentAngle){
        
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    
    }
}