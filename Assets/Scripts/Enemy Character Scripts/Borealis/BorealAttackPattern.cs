using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorealAttackPattern : MonoBehaviour
{
    private BorealCharacterController cont;
    private int attackCount = 0;
    private bool isAttack = false;
    private bool returnOrigin = false;
    

    void Awake()
    {
        cont = GetComponent<BorealCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(returnOrigin){
            cont.returnToOrigin();
            if(cont.atOrigin()){
                returnOrigin = false;
            }
        }

        if ((attackCount == 0 && !isAttack) && !returnOrigin){
            isAttack = true;
            StartCoroutine(startTimer(attackCount, "wave"));
        } else if ((attackCount == 0 && isAttack) && !returnOrigin){
            cont.FlyWave();
        } 

        if((attackCount == 1 && !isAttack) && !returnOrigin){
            isAttack  = true;
            StartCoroutine(startTimer(attackCount, "egg"));
        }  else if ((attackCount == 1 && isAttack) && !returnOrigin){
            cont.FlyEgg();
        }

        if((attackCount == 2 && !isAttack) && !returnOrigin){
            isAttack = true;
            StartCoroutine(startTimer(attackCount, "egg"));
        } else if ((attackCount == 2 && isAttack) && !returnOrigin){
            cont.FlyEgg();
        }

        if((attackCount == 3 && !isAttack) && !returnOrigin){
            isAttack = true;
            StartCoroutine(startTimer(attackCount, "wave"));
        } else if ((attackCount == 3 && isAttack) && !returnOrigin){
            cont.FlyWave();
        }

        if((attackCount == 4 && !isAttack) && !returnOrigin){
            isAttack = true;
            StartCoroutine(startTimer(attackCount, "egg"));
        } else if ((attackCount == 4 && isAttack) && !returnOrigin){
            cont.FlyEgg();
        }
        
    }

    private IEnumerator startTimer(int currentAttack, string attack){
        float attackLength = 0f;
        if(attack == "dive"){
            attackLength = 5f;
        }  else if (attack == "egg"){
            attackLength = 4f;
        } else if (attack == "wave"){
            attackLength = 4f;
        }
        yield return new WaitForSeconds(attackLength);
        if(currentAttack < 4){
            attackCount++;
        } else {
            attackCount = 0;
        }
        isAttack = false;
        returnOrigin = true;

    }


    
}
