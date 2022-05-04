using System.Collections;
using UnityEngine;

public class AttackNew : MonoBehaviour
{
    [SerializeField] private Transform attack;
    [SerializeField] private int damageAmount;
    [SerializeField] public EnumCharacterAnimationState animationState;
    public LayerMask enemy;

    public void Hit()
    { // Checks for objects on the enemy layer and if there is one in the area around the weapon , does damage
        Collider[] hit = Physics.OverlapSphere(attack.position, 3.0f, enemy, QueryTriggerInteraction.Collide);
        //can make the weapon radius into a variable

        foreach (Collider col in hit)
        {
            col.gameObject.GetComponent<Health>().TakeDamage(damageAmount); //Change the damage passed in to a variable
        }
    }
}