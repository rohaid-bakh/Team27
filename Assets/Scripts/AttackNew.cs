using System.Collections;
using UnityEngine;

public class AttackNew : MonoBehaviour
{
    [SerializeField] private Transform attack;
    [SerializeField] private int baseDamageAmount;
    [SerializeField] public EnumCharacterAnimationStateName animationState;
    public LayerMask enemy;

    public void Hit()
    { // Checks for objects on the enemy layer and if there is one in the area around the weapon , does damage
        Collider[] hit = Physics.OverlapSphere(attack.position, 3.0f, enemy, QueryTriggerInteraction.Collide);
        //can make the weapon radius into a variable

        foreach (Collider col in hit)
        {
            int damageAmount = baseDamageAmount;
            CharacterMonoBehaviour character = col.gameObject.GetComponent<CharacterMonoBehaviour>();
            if(character != null)
            {
                // check if character was blocking. If so, reduce amount of damage by x% (let's say 75% less?)
                if (character.IsBlocking()) damageAmount = (int)((float)damageAmount * 0.25f);
                character.TakeDamage();
            }
            col.gameObject.GetComponent<Health>().TakeDamage(damageAmount); //Change the damage passed in to a variable
        }
    }
}