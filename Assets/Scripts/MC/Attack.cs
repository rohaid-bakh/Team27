using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private Control control;
    [SerializeField]
    private Transform attack;
    public LayerMask enemy;

    private void Awake(){
        control = new Control();
        control.Enable();
        control.Action.Enable();
        control.Action.Attack.Enable();

        control.Action.Attack.performed += _ => Hit();
    }

    private void Hit(){ // Checks for objects on the enemy layer and if there is one in the area around the weapon , does damage
       Collider[] hit = Physics.OverlapSphere(attack.position, 3.0f , enemy, QueryTriggerInteraction.Collide);
       //can make the weapon radius into a variable

       foreach(Collider col in hit){
           col.gameObject.GetComponent<Health>().TakeDamage(2); //Change the damage passed in to a variable
       }
    }
}
