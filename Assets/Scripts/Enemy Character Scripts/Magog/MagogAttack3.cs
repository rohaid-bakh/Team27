using System.Collections;
using UnityEngine;

// This attack is the standard basic attack (will swipe at player)
public class MagogAttack3 : MonoBehaviour, IAttack
{
    [SerializeField] int damageAmount = 5;

    bool isCharging = false;

    MagogCharacterController characterController;

    void Start()
    {
        characterController = GetComponentInParent<MagogCharacterController>();
    }

    public void Attack()
    {
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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Charging");

        if (isCharging)
        {
            // check for collision with player
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                CharacterMonoBehaviour playerCharacter = other.gameObject.GetComponent<CharacterMonoBehaviour>();
                if (playerCharacter != null)
                {
                    playerCharacter.TakeDamage(damageAmount);
                }
            }
        }
    }
}