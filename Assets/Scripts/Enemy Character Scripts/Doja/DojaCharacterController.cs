using System;
using System.Collections;
using UnityEngine;

public enum EnumDojaFightLoopState
{
    BiteAttack,
    ClawAttack,
    DashAttack
}
public class DojaCharacterController : EnemyCharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    DojaAttack1 biteAttack;
    DojaAttack2 clawAttack;
    DojaAttack3 dashAttack;

    // keep track of current state in the fight
    EnumDojaFightLoopState nextState = EnumDojaFightLoopState.BiteAttack;

    // serialize field
    [SerializeField] float dashSpeed = 2f;

    // start/end transforms of map
    [SerializeField] Transform leftEndPoint;
    [SerializeField] Transform rightEndPoint;

    [Header("Rage Mode Details")]
    [Range(0, 1)]
    [SerializeField] float healthPercentageToEnterRage = 0.3f; // how low should health be to enter rage state
    [SerializeField] float speedModifierOnRage = 1.5f; // what to multiply the speed by when in rage
    [SerializeField] float idleTimeModifierOnRage = 0.75f; // what to multiply the idle time by when in rage
    [SerializeField] Color rageColor; // temporary to flag if the magog is in rage
    [SerializeField] SpriteRenderer magogSpriteRenderer;
    bool inRageMode = false;
    bool enteringRageMode = false;

    // to keep track of current coroutine
    Coroutine enemyLoopCoroutine = null;
    Coroutine enemyRageCoroutine = null;

    // keep track of max health

    int maxHealth;

    private void Start()
    {
        biteAttack = GetComponentInChildren<DojaAttack1>();
        clawAttack = GetComponentInChildren<DojaAttack2>();
        dashAttack = GetComponentInChildren<DojaAttack3>();
        enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());

        maxHealth = GetMaxHealthStat();
    }
    
    // standard enemy behaviour loop
    private IEnumerator EnemyAIBehaviourLoop1()
    {
        // loop until enemy is dead
        while (IsDead() == false)
        {
            //testing
            nextState = EnumDojaFightLoopState.DashAttack; // EnumDojaFightLoopState.BiteAttack; //

            // actions based on current fight state
            switch (nextState)
            {
                case EnumDojaFightLoopState.BiteAttack:
                    yield return BiteAttackStage();
                    break;
                case EnumDojaFightLoopState.ClawAttack:
                    yield return ClawAttackStage();
                    break;
                case EnumDojaFightLoopState.DashAttack:
                    yield return DashAttackStage();
                    break;
            }
        }

        yield return new WaitForSeconds(0f); // to avoid complaints about not all code paths return value
    }

    #region Enemy Fight Actions - Loop 
    private IEnumerator BiteAttackStage()
    {
        // swipe three times
        for(int i = 0; i < 4; i++)
        {
            yield return BiteAttack();
        }

        // idle 1 second
        yield return Idle(1f);

        // update the next state
        nextState = EnumDojaFightLoopState.ClawAttack;
    }

    private IEnumerator ClawAttackStage()
    {
        // charge attack twice
        for(int i =0; i < 2; i++)
        {
            // charge
            yield return ClawAttack();
        }
        
        // update the next state
        nextState = EnumDojaFightLoopState.DashAttack;
    }

    private IEnumerator DashAttackStage()
    {
        // charge attack twice
        for (int i = 0; i < 4; i++)
        {
            // charge
            yield return DashAttack();
        }

        yield return Idle(1);

        // update the next state
        nextState = EnumDojaFightLoopState.BiteAttack;
    }
    
    IEnumerator EnterRageMode()
    {
        Debug.Log("Entering Rage Mode");

        if (!inRageMode)
        {
            enteringRageMode = true;
            inRageMode = true;

            // stop any current attack animations that are playing 
            FinishAttackAnimation();

            // stop corouting
            StopCoroutine(enemyLoopCoroutine);

            // stop moving
            Move(Vector2.left);

            // face player
            yield return FacePlayer();

            PlayAnimation(EnumCharacterAnimationStateName.EnterRage);

            // wait for animation to finish
            yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);

            // increase speed
            baseSpeedModifier = speedModifierOnRage;
            idleTimeModifier = idleTimeModifierOnRage;

            // set visual indicator (ex. set colour to red)
            magogSpriteRenderer.color = rageColor;

            enteringRageMode = false;

            enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());

            StopCoroutine(enemyRageCoroutine);
        }
    }
    #endregion

    #region Attacks
    IEnumerator BiteAttack()
    {
        // move towards player and swipe
        MoveTowardsPlayer();

        yield return new WaitForSeconds(0.5f);

        // attack1 swipes at player
        Attack(biteAttack);

        // wait for attack animation to finish
        yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);
    }

    IEnumerator ClawAttack()
    {
        //todo
        yield return new WaitForSeconds(0f);
    }
    
    IEnumerator DashAttack()
    {
        yield return FacePlayer();

        yield return Idle(1f);

        yield return Dash();

        yield return Idle(1f);
    }
    
    #endregion

    #region Override
    public override void TakeDamage(int damageAmount)
    {
        // don't take damage when entering rage
        if (enteringRageMode == false)
        {
            // take damage
            bool isCharacterDead = ApplyDamageToHealth(damageAmount);
            if (isCharacterDead)
            {
                StopCoroutine(enemyLoopCoroutine);
                SetState(new DeadCharacterState());
            }

            // check if health is below a certain point to enter rage
            if (!inRageMode)
            {
                int currentHealth = characterHealth.GetCurrentHealth();
                float healthPercentage = (float)currentHealth / (float)maxHealth;

                Debug.Log($"{currentHealth}/{maxHealth} : {healthPercentage} <= {healthPercentageToEnterRage}");

                if (healthPercentage <= healthPercentageToEnterRage)
                {
                    Debug.Log("Starting Rage Coroutine");
                    if(enemyRageCoroutine == null)
                        enemyRageCoroutine = StartCoroutine(EnterRageMode());
                }
            }
        }
    }

    #endregion

    #region Helper functions

    IEnumerator Dash()
    {
        float direction = Mathf.Sign(transform.localScale.x);
        float attackTime = (float)characterAnimator.GetAnimationClipTime((EnumCharacterAnimationStateName)biteAttack.GetAnimationStateName());

        Move(Vector2.zero);

        // pause regular movement when dashing
        SetCanMoveBool(false);

        Attack(biteAttack);

        float timePassed = 0;
        while (timePassed < attackTime)
        {
            // Code to go left here
            rigidBody.velocity = new Vector2(direction * dashSpeed, rigidBody.velocity.y);

            timePassed += Time.deltaTime;

            yield return null;
        }

        SetCanMoveBool(true);
    }

    #endregion
}