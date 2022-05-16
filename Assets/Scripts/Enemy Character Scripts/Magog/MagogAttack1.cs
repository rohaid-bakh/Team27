using System.Collections;
using UnityEngine;

// This attack is the standard basic attack (will swipe at player)
public class MagogAttack1 : MonoBehaviour, IAttack
{
    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack1;
    public EnumSoundName? GetSoundEffectName() => EnumSoundName.MagogAttack;

    public int DamageAmount
    {
        get
        {
            return damageAmount;
        }
        set
        {
            damageAmount = value;
        }
    }

    [SerializeField] private Transform attackPoint;
    [SerializeField] int damageAmount = 1;
    [SerializeField] float attackRadius = 0.25f;
    public LayerMask enemy;

    CharacterMonoBehaviour characterController;

    bool enemyHit;

    void Start()
    {
        characterController = GetComponentInParent<CharacterMonoBehaviour>();
    }

    // this function will draw a red sphere in the "Scene" view to show the attack position & radius
    private void OnDrawGizmos()
    {
        // for testing to see size of sphere (attack radius)
        Gizmos.color = Color.red;
        //Use the same vars you use to draw your Overlap SPhere to draw your Wire Sphere.
        Gizmos.DrawWireSphere(attackPoint.transform.position, attackRadius);
    }

    public void Attack()
    {
        enemyHit = false;
        StartCoroutine(SwipeAttack());
    }

    public IEnumerator SwipeAttack()
    {
        enemyHit = false;

        // keep attacking while character is in the attack state/animation and if they haven't already hit the enemy yet
        while (characterController.IsAttacking() == true && !enemyHit)
        {
            // keep attacking / checking if enemy is within radius
            enemyHit = CheckIfCharacterHit();

            yield return new WaitForSeconds(0f);
        }
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
}