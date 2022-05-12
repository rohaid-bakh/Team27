using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorealAttackPattern : MonoBehaviour
{
    private BorealCharacterController cont;
    private int attackCount = 0;
    private bool isAttack = false;
    void Awake()
    {
        cont = GetComponent<BorealCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attackCount == 0 && !isAttack){
            isAttack = true;
            StartCoroutine(startTimer(attackCount));
        } else if (attackCount == 0 && isAttack){
            cont.FlyWave();
        }

        if(attackCount == 1 && !isAttack){
            isAttack  = true;
            StartCoroutine(startTimer(attackCount));
        }  else if (attackCount == 1 && isAttack){
            cont.FlyDive();
        }

        if(attackCount == 2 && !isAttack){
            isAttack = true;
            StartCoroutine(startTimer(attackCount));
        } else if (attackCount == 2 && isAttack){
            cont.FlyEgg();
        }

        if(attackCount == 3 && !isAttack){
            isAttack = true;
            StartCoroutine(startTimer(attackCount));
        } else if (attackCount == 3 && isAttack){
            cont.FlyDive();
        }

        if(attackCount == 4 && !isAttack){
            isAttack = true;
            StartCoroutine(startTimer(attackCount));
        } else if (attackCount == 4 && isAttack){
            cont.FlyWave();
        }
        
    }

    private IEnumerator startTimer(int currentAttack){
        yield return new WaitForSeconds(2f);
        if(currentAttack < 4){
            attackCount++;
        } else {
            attackCount = 0;
        }
        isAttack = false;
    }
}
