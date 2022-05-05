using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    private int hpbar;
    public Stats stat;
    [SerializeField]
    private Slider healthbar;
    void Awake(){
        hpbar = stat.health;
        healthbar.normalizedValue = 1;
    }
    public void TakeDamage(int damage){ // other classes call this
        hpbar -= damage;
        healthbar.normalizedValue = 1f-((float)(stat.health-hpbar)/(float)stat.health);
        if(hpbar <= 0){
            Death();
        }
    }

    private void Death(){
        // place holder until we have a death script/idea.
        Debug.Log("This character has died.");
    }
}
