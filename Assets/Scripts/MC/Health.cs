using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int hpbar;
    public Stats stat;
    void Awake(){
        hpbar = stat.health;
    }
    public void TakeDamage(int damage){ // other classes call this
        hpbar -= damage;
        if(hpbar <= 0){
            Death();
        }
    }

    private void Death(){
        // place holder until we have a death script/idea.
        Debug.Log("This character has died.");
    }
}
