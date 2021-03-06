using System.Collections;
using UnityEngine;

public class BaseAttack : MonoBehaviour, IAttack
{

    [SerializeField] private Transform attackPoint; 
    [SerializeField] public int damageAmount = 1;
    [SerializeField] float attackRadius = 0.25f;
    public LayerMask enemy;

    public virtual EnumCharacterAnimationStateName? GetAnimationStateName() => null;
    public virtual EnumSoundName? GetSoundEffectName() => null;

    // this function will draw a red sphere in the "Scene" view to show the attack position & radius
    private void OnDrawGizmos()
    {
        // for testing to see size of sphere (attack radius)
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }

    // for custom behaviour you can override this 
    public void Attack()
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
            }
            else
            {
                Health health = col.gameObject.GetComponent<Health>();
                if(health != null)
                {
                    Debug.Log("Character monobehaviour not found for attack damage. Using Health.TakeDamage directly");

                    // ie. if no charactermonobehavior, search for health component & apply the damage directly
                    col.gameObject.GetComponent<Health>()?.TakeDamage(damageAmount);
                }
            }
        }
    }

}