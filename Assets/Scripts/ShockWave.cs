using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    float timer;
    float maxTimer = 3.0f;
    private int direct;
    void FixedUpdate(){
        timer += Time.fixedDeltaTime;
        transform.position = new Vector3(transform.position.x + (direct* Time.fixedDeltaTime * 10f), transform.position.y , transform.position.z);
        if(timer >= maxTimer){
            Destroy(gameObject);
        }
    }

    public void ProjectileDirection(int direction){
        direct = direction;
    }  
}
