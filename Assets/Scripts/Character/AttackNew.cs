﻿using System.Collections;
using UnityEngine;

public class AttackNew : MonoBehaviour
{
    [SerializeField] private Transform attack;
    [SerializeField] private int damageAmount;
    [SerializeField] private float attackRadius;
    [SerializeField] public EnumCharacterAnimationStateName animationState;
    [SerializeField] public EnumSoundName attackSoundEffect; 
    public LayerMask enemy;

    // this function will draw a red sphere in the "Scene" view to show the attack position & radius
    private void OnDrawGizmos()
    {
        // for testing to see size of sphere (attack radius)
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(attack.transform.position, attackRadius);
    }

    // for custom behaviour you can override the 
    public void Hit()
    { // Checks for objects on the enemy layer and if there is one in the area around the weapon , does damage
        Collider[] hit = Physics.OverlapSphere(attack.position, 1f, enemy, QueryTriggerInteraction.Collide);

        foreach (Collider col in hit)
        {
            // note: this only works if all of the enemies inherit from CharacterMonobehavior. Otherwise need to use GetComponent<Health>().TakeDamage();
            CharacterMonoBehaviour character = col.gameObject.GetComponent<CharacterMonoBehaviour>();
            if(character != null)
            {
                character.TakeDamage(damageAmount);
            }
        }
    }
}