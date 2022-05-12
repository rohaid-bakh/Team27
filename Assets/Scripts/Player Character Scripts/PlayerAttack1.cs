using System;
using System.Collections;
using UnityEngine;

public class PlayerAttack1 : MonoBehaviour, IAttack
{
    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack1;
    public EnumSoundName? GetSoundEffectName() => EnumSoundName.PlayerAttack1;

    [SerializeField] private Transform attackPoint;
    [SerializeField] public int damageAmount = 1;
    [SerializeField] float attackRadius = 0.25f;
    public LayerMask enemy;

    PlayerCharacterController characterController;

    bool enemyHit; 

    void Start()
    {
        characterController = GetComponentInParent<PlayerCharacterController>();
    }

    public void Attack()
    {
        enemyHit = false;
        StartCoroutine(SwordAttack());
    }

    public IEnumerator SwordAttack()
    {
        enemyHit = false;
        Debug.Log("Swinging");

        // keep attacking while character is in the attack state/animation and if they haven't already hit the enemy yet
        while(characterController.IsAttacking() == true && !enemyHit)
        {
            // keep attacking / checking if enemy is within radius
            enemyHit = CheckIfCharacterHit();

            yield return new WaitForSeconds(0f);
        }

        Debug.Log("Finished Swinging");
    }

    // for custom behaviour you can override this. Returns true if hit landed, returns false if no hit landed
    public virtual bool CheckIfCharacterHit()
    { // Checks for objects on the enemy layer and if there is one in the area around the weapon , does damage
        Collider[] hit = Physics.OverlapSphere(attackPoint.position, attackRadius, enemy, QueryTriggerInteraction.Collide);

        foreach (Collider col in hit)
        {
            // note: this only works if all of the "opponent" inherits from CharacterMonobehavior. Otherwise need to use GetComponent<Health>().TakeDamage();
            CharacterMonoBehaviour character = col.gameObject.GetComponent<CharacterMonoBehaviour>();
            if (character != null)
            {
                //Debug.Log($"Character state: {character.GetState()}");
                character.TakeDamage(damageAmount);
                return true;
            }
            else
            {
                Health health = col.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    Debug.Log("Character monobehaviour not found for attack damage. Using Health.TakeDamage directly");

                    // ie. if no charactermonobehavior, search for health component & apply the damage directly
                    col.gameObject.GetComponent<Health>()?.TakeDamage(damageAmount);
                    return true;
                }
            }
        }

        return false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    Debug.Log("TriggerStay: " + enemyHit);

    //    // check if enemy hasn't been hit yet (only 1 hit per attack) & if collision with enemy
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && !enemyHit)
    //    {
    //        Debug.Log("Trigger collision with enemy");
    //        enemyHit = true;

    //        CharacterMonoBehaviour enemyCharacter = other.gameObject.GetComponent<CharacterMonoBehaviour>();
    //        if (enemyCharacter != null)
    //        {
    //            // apply damage
    //            enemyCharacter.TakeDamage(damageAmount);
    //        }
    //        else // if enemy doesn't inherit from CharacterMonoBehaviour
    //        {
    //            Health health = other.gameObject.GetComponent<Health>();
    //            if (health != null)
    //            {
    //                Debug.Log("Character monobehaviour not found for attack damage. Using Health.TakeDamage directly");

    //                // ie. if no charactermonobehavior, search for health component & apply the damage directly
    //                other.gameObject.GetComponent<Health>()?.TakeDamage(damageAmount);
    //            }
    //        }
    //    }
    //}
}