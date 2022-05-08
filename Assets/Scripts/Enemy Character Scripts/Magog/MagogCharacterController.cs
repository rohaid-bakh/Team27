using System;
using System.Collections;
using UnityEngine;

public enum EnumMagogFightLoopState
{
    SwipeAttack,
    ProjectileAttack,
    ChargeAttack
}
public class MagogCharacterController : EnemyCharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    MagogAttack1 swipeAttacl;
    MagogAttack2 projectileAttack;
    MagogAttack3 chargeAttack;

    // keep track of current state in the fight
    EnumMagogFightLoopState nextState = EnumMagogFightLoopState.SwipeAttack;

    // serialize field
    [SerializeField] float chargeSpeedModifier = 2f;

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
        swipeAttacl = GetComponentInChildren<MagogAttack1>();
        projectileAttack = GetComponentInChildren<MagogAttack2>();
        chargeAttack = GetComponentInChildren<MagogAttack3>();
        enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());

        maxHealth = GetMaxHealthStat();
    }
    
    // standard enemy behaviour loop
    private IEnumerator EnemyAIBehaviourLoop1()
    {
        yield return FacePlayer();
        yield return new WaitForSeconds(1f);
        MoveTowardsPlayer();
        yield return new WaitForSeconds(1f);

        // loop until enemy is dead
        while(IsDead() == false)
        {
            //testing
            //nextState = EnumMagogFightLoopState.ProjectileAttack;

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
        for(int i = 0; i < 4; i++)
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

        yield return Idle(2);

        // update the next state
        nextState = EnumMagogFightLoopState.SwipeAttack;
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

        // move towards player
        Move(playerDirection, chargeSpeedModifier * baseSpeedModifier);

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
        // fire six projectiles
        for(int i = 0; i < 6; i++)
        {
            MoveTowardsPlayer();

            // use attack2 shoots projectiles
            PlayAttackAnimation(EnumCharacterAnimationStateName.Attack2);

            Attack(projectileAttack);

            FinishAttackAnimation();

            yield return new WaitForSeconds(1f);
        }
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
}