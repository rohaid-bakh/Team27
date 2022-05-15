using System;
using System.Collections;
using UnityEngine;

public enum EnumDojaFightLoopState
{
    BiteAttack,
    ClawAttack
}
public class DojaCharacterController : EnemyCharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    DojaAttack1 biteAttack;
    DojaAttack2 clawAttack;

    // keep track of current state in the fight
    EnumDojaFightLoopState nextState = EnumDojaFightLoopState.BiteAttack;

    // serialize field
    [SerializeField] float clawSweepSpeed = 2f;

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
            //nextState = EnumDojaFightLoopState.DashAttack; // EnumDojaFightLoopState.BiteAttack; //

            // actions based on current fight state
            switch (nextState)
            {
                case EnumDojaFightLoopState.BiteAttack:
                    yield return BiteAttackStage();
                    break;
                case EnumDojaFightLoopState.ClawAttack:
                    yield return ClawAttackStage();
                    break;
            }

            //MoveTowardsPlayer();

            //yield return new WaitForSeconds(3f);

            //yield return Idle(5);

            //yield return Idle(30f);
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

        // update the next state
        nextState = EnumDojaFightLoopState.ClawAttack;

        // idle 1 second
        yield return Idle(2f);
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
        nextState = EnumDojaFightLoopState.ClawAttack;

        // idle 1 second
        yield return Idle(2f);
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
            FacePlayer();

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
        // move towards player and swipe
        MoveTowardsPlayer(clawSweepSpeed);

        yield return new WaitForSeconds(0.5f);

        // attack1 swipes at player
        Attack(clawAttack);

        // wait for attack animation to finish
        yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);
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

                //Debug.Log($"{currentHealth}/{maxHealth} : {healthPercentage} <= {healthPercentageToEnterRage}");

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

    #region Retaliate
    //private void OnCollisionEnter(Collision collision)
    //{
    //    // check for collision with player
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        FacePlayer();
    //        Attack(biteAttack);
    //    }
    //}

    //IEnumerator RetaliateAttack()
    //{

    //}
    #endregion
}