using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private Collider col;
    void Awake(){
        col = GetComponent<Collider>();
    }

   private void OnTriggerEnter(Collider other)  {
       // TODO: add a coroutine to lag hit 
       // TODO: add screenshake
       // TODO: add flash hit
        if(other.gameObject.tag == "Damage"){ // if it's a player taking damage
            GetComponent<Health>().TakeDamage(2); //replace the damage with stat variable
            Debug.Log("Hit!");
        }
        
    }
}
