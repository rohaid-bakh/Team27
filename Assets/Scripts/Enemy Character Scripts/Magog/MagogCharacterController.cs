using System;
using System.Collections;
using UnityEngine;

public enum EnumMagogFightLoopState
{
    Idle1,
    SwipeAttack,
    Idle2,
    ProjectileAttack
}
public class MagogCharacterController : CharacterMonoBehaviour
{
    // could have a multiple attacks, just need to serialize and add them here
    MagogAttack1 attack1;
    MagogAttack2 attack2;

    // to keep track of current coroutine
    Coroutine enemyLoopCoroutine = null;

    // keep track of current state in the fight
    EnumMagogFightLoopState nextState = EnumMagogFightLoopState.Idle1;

    // the player transform
    [SerializeField] Transform playerTransform;

    private void Start()
    {
        attack1 = GetComponentInChildren<MagogAttack1>();
        attack2 = GetComponentInChildren<MagogAttack2>();
        enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());
    }
    
    // standard enemy behaviour loop (could also implement different loops for different stages of the fight)
    private IEnumerator EnemyAIBehaviourLoop1()
    {
        // loop until enemy is dead
        while(IsDead() == false)
        {
            // actions based on current fight state
            switch (nextState)
            {
                case EnumMagogFightLoopState.Idle1:
                    yield return Idle1();
                    break;
                case EnumMagogFightLoopState.SwipeAttack:
                    yield return SwipeAtPlayer();
                    break;
                case EnumMagogFightLoopState.Idle2:
                    yield return Idle2();
                    break;
                case EnumMagogFightLoopState.ProjectileAttack:
                    yield return ProjecttileAttack();
                    break;
            }

            yield return new WaitForSeconds(0f); // to avoid complaints about not all code paths return value
        }

        yield return new WaitForSeconds(0f); // to avoid complaints about not all code paths return value
    }

    #region Enemy Fight Actions - Loop 
    private IEnumerator Idle1()
    {
        // Idle
        Move(Vector2.zero);

        yield return new WaitForSeconds(2f);

        nextState = EnumMagogFightLoopState.SwipeAttack;
    }
    private IEnumerator Idle2()
    {
        // Idle
        Move(Vector2.zero);

        yield return new WaitForSeconds(2f);

        nextState = EnumMagogFightLoopState.ProjectileAttack;
    }
    private IEnumerator SwipeAtPlayer()
    {
        // move towards player and swipe
        MoveTowardsPlayer();

        yield return new WaitForSeconds(2f);

        // attack1 swipes at player
        Attack(attack1);

        // wait for attack animation to finish
        yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);

        // update the next state
        nextState = EnumMagogFightLoopState.Idle2;
    }
    private IEnumerator ProjecttileAttack()
    {
        // move towards player
        MoveTowardsPlayer();

        yield return new WaitForSeconds(2f);

        // stop moving & use attack2 shoots projectiles
        Move(Vector2.zero);
        Attack(attack2);

        // wait for attack animation to finish
        yield return new WaitUntil(() => IsWaitingForAnimationToFinish() == false);

        // update the next state
        nextState = EnumMagogFightLoopState.Idle1;
    }
    #endregion

    #region Interupts/Retaliations (anything that might break the enemy out of their standatd loop)
    // this needs work!!
    public override void TakeDamage(int damageAmount)
    {
        // stop coroutine
        StopCoroutine(enemyLoopCoroutine);

        // stop any movement
        Move(Vector3.zero);

        // take damage
        base.TakeDamage(damageAmount);

        // retaliate attack (if not attacking already)
        if(!IsAttacking())
            StartCoroutine(RetaliateAttack());
    }

    IEnumerator RetaliateAttack()
    {
        // wait half a second
        yield return new WaitForSeconds(1f);

        Debug.Log("Retaliating!");

        // retaliate by attacking player
        Attack(attack1);

        // resume coroutine
        enemyLoopCoroutine = StartCoroutine(EnemyAIBehaviourLoop1());
    }

    #endregion

    #region Helper Functions
    // override flip sprite 
    public override void FlipSprite()
    {
        // always face player
        float playerDirection = GetPlayerDirection().x;
        transform.localScale = new Vector2(Mathf.Sign(playerDirection), 1f);
    }

    // overried move to always move towards player
    public override void Move()
    {
        if (GetCanMoveBool())
        {
            float targetDirection = Mathf.Sign(GetPlayerDirection().x) * Mathf.Abs(GetMoveInputX());
            Vector3 characterVelocity = new Vector3(targetDirection * GetMoveSpeed(), rigidBody.velocity.y, rigidBody.velocity.z);
            rigidBody.velocity = characterVelocity;

            FlipSprite();
        }
    }


    Vector2 GetPlayerDirection()
    {
        // if the player x position is greater than the current enemy position
        if (playerTransform.position.x > transform.position.x)
            return Vector2.right;
        else
            return Vector2.left;
    }

    void MoveTowardsPlayer()
    {
        Vector2 playerDirection = GetPlayerDirection();
        Move(playerDirection);
    }

    #endregion
}