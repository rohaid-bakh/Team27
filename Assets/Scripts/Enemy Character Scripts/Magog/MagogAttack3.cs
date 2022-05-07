using System.Collections;
using UnityEngine;

// This attack is the standard basic attack (will swipe at player)
public class MagogAttack3 : MonoBehaviour, IAttack
{
    [SerializeField] int damageAmount = 5;
    [SerializeField] float durationToTurnOffCharacterCollisionOnImpact = 1f;
    [SerializeField] Vector3 knockBackForce;

    bool isCharging = false;
    bool playerHit = false;

    MagogCharacterController characterController;

    void Start()
    {
        characterController = GetComponentInParent<MagogCharacterController>();
    }

    public void Attack()
    {
        playerHit = false;
        StartCoroutine(ChargeAttack());
    }

    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack3;
    public EnumSoundName? GetSoundEffectName() => null; 

    public IEnumerator ChargeAttack()
    {
        isCharging = true;

        // wait until character is done chargin
        yield return new WaitUntil(() => characterController.IsAttacking() == false);

        isCharging = false;
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Charging");

        // check if charging & if player hasn't been hit yet (only 1 hit per charge attack)
        if (isCharging & !playerHit)
        {
            // check for collision with player
            if (other.CompareTag("Player"))
            {
                playerHit = true;

                CharacterMonoBehaviour playerCharacter = other.gameObject.GetComponent<CharacterMonoBehaviour>();
                if (playerCharacter != null)
                {
                    // temporarily turn off collisions so the player isn't pushed off screen
                    StartCoroutine(playerCharacter.IgnoreCollisionTemporarily(characterController.characterCollider, durationToTurnOffCharacterCollisionOnImpact));

                    // todo: maybe add knock back force to player?

                    // apply damage
                    playerCharacter.TakeDamage(damageAmount);
                }
            }
        }
    }
}