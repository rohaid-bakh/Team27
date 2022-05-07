using System;
using System.Collections;
using UnityEngine;

public enum EnumMagogFightLoopState
{
    SwipeAttack,
    ChargeAttack,
    ProjectileAttack
}
public class MagogCharacterController : CharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    MagogAttack1 swipeAttacl;
    MagogAttack2 projectileAttack;
    MagogAttack3 chargeAttack;

    // keep track of current state in the fight
    EnumMagogFightLoopState nextState = EnumMagogFightLoopState.SwipeAttack;

    // serialize field
    [SerializeField] float chargeSpeed = 2f;

    // the player transform
    [SerializeField] Transform playerTransform;

    // start/end transforms of map
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightEndPoint;

    // to keep track of current coroutine
    Coroutine enemyLoopCoroutine = null;

    private void Start()
    {
        swipeAttacl = GetComponentInChildren<MagogAttack1>();
        projectileAttack = GetComponentInChildren<MagogAttack2>();
        chargeAttack = GetComponentInChildren<MagogAttack3>();
        enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());
    }
    
    // standard enemy behaviour loop
    private IEnumerator EnemyAIBehaviourLoop1()
    {
        yield return FacePlayer();
        yield return new WaitForSeconds(2);

        // loop until enemy is dead
        while(IsDead() == false)
        {
            // actions based on current fight state
            switch (nextState)
            {
                case EnumMagogFightLoopState.SwipeAttack:
                    yield return SwipeAttackStage();
                    break;
                case EnumMagogFightLoopState.ChargeAttack:
                    yield return ChargePlayerAttackStage();
                    break;
                case EnumMagogFightLoopState.ProjectileAttack:
                    yield return ProjecttileAttackStage();
                    break;
            }
        }

        yield return new WaitForSeconds(0f); // to avoid complaints about not all code paths return value
    }

    #region Enemy Fight Actions - Loop 
    private IEnumerator SwipeAttackStage()
    {
        // swipe three times
        for(int i = 0; i < 3; i++)
        {
            yield return SwipeAttack();
        }

        // idle 1 second
        yield return Idle(1f);

        // update the next state
        nextState = EnumMagogFightLoopState.ChargeAttack;
    }

    private IEnumerator ChargePlayerAttackStage()
    {
        // charge attack twice
        for(int i =0; i < 2; i++)
        {
            // charge
            yield return ChargePlayerAttack();
        }
        
        // update the next state
        nextState = EnumMagogFightLoopState.ProjectileAttack;
    }

    private IEnumerator ProjecttileAttackStage()
    {
        // face towards player
        MoveTowardsPlayer();

        // projectile
        yield return ProjectileAttack();

        yield return Idle(1);

        // update the next state
        nextState = EnumMagogFightLoopState.SwipeAttack;
    }
    #endregion

    #region Attacks
    IEnumerator SwipeAttack()
    {
        // move towards player and swipe
        MoveTowardsPlayer();

        yield return new WaitForSeconds(0.5f);

        // attack1 swipes at player
        Attack(swipeAttacl);

        // wait for attack animation to finish
        yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);
    }

    IEnumerator ChargePlayerAttack()
    {
        // face player
        yield return FacePlayer();

        // get target transofrm position
        Vector2 playerDirection = GetPlayerDirection();
        Transform targetPosition = playerDirection == Vector2.left ? leftEndPoint : rightEndPoint;

        // animation
        PlayAttackAnimation(EnumCharacterAnimationStateName.Attack3);

        // idle
        yield return Idle(1.5f);

        // move towards left side of map
        Move(playerDirection, chargeSpeed);

        // attack
        Attack(chargeAttack);

        // wait until enemy reaches target position on other side of arena
        yield return new WaitUntil(() => Mathf.Abs((transform.position.x) - (targetPosition.position.x)) < 2);

        // stop moving
        Move(Vector2.zero);

        FinishAttackAnimation();

        // Idle
        yield return Idle(1.5f);
    }
    
    IEnumerator ProjectileAttack()
    {
        // use attack2 shoots projectiles
        PlayAttackAnimation(EnumCharacterAnimationStateName.Attack2);

        Attack(projectileAttack);

        yield return new WaitForSeconds(6f);

        FinishAttackAnimation();
    }
    #endregion

    #region Override
    public override void TakeDamage(int damageAmount)
    {
        // take damage
        bool isCharacterDead = ApplyDamageToHealth(damageAmount);
        if (isCharacterDead)
            SetState(new DeadCharacterState());
    }

    #endregion

    #region Helper Functions
    IEnumerator FacePlayer()
    {
        // To face player, move towards them
        MoveTowardsPlayer();

        yield return new WaitForSeconds(0.1f);

        // Stop moving
        Move(Vector2.zero);
    }
    IEnumerator Idle(float numberOfSeconds)
    {
        // Idle
        Move(Vector2.zero);

        yield return new WaitForSeconds(numberOfSeconds);
    }

    void PlayAttackAnimation(EnumCharacterAnimationStateName attackAnimation)
    {
        characterAnimator.ChangeAnimationState(attackAnimation);
        characterAnimator.SetWaitForAnimationToComplete(true);
    }

    void FinishAttackAnimation()
    {
        characterAnimator.SetWaitForAnimationToComplete(false);
    }

    Vector2 GetPlayerDirection()
    {
        // if the player x position is greater than the current enemy position
        if (playerTransform.position.x > transform.position.x)
            return Vector2.right;
        else
            return Vector2.left;
    }

    void MoveTowardsPlayer(float speedModifier = 1)
    {
        Vector2 playerDirection = GetPlayerDirection();
        Move(playerDirection, speedModifier);
    }


    #endregion
}