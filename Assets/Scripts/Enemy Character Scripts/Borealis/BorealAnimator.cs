using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorealAnimator : MonoBehaviour
{
    [SerializeField] private Animator anim ;
    private BorealAttackPattern attack;
    private BorealCharacterController cont;
    private Health health;

    void Awake(){
        health = GetComponent<Health>();
        attack = GetComponent<BorealAttackPattern>();
        cont = GetComponent<BorealCharacterController>();
    }

    void Update(){
        if(health.GetCurrentHealth() == 0){
            
            anim.enabled = true;
            attack.notDead = false;
            cont.returnToOrigin();
        }
    }

   
}
