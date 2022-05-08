using System;
using System.Collections;
using UnityEngine;

public class EnemyCharacterMonoBehaviour : CharacterMonoBehaviour
{
    // speed / timing
    [HideInInspector] public float baseSpeedModifier = 1;
    [HideInInspector] public float idleTimeModifier = 1;

    // keep track of positions in the arena (player and end points of arena)
    [Header("Transform Positions")]
    // the player transform
    [SerializeField] public Transform playerTransform;

    #region Helper Functions
    public IEnumerator FacePlayer()
    {
        // To face player, move towards them
        MoveTowardsPlayer();

        yield return new WaitForSeconds(0.1f);

        // Stop moving
        Move(Vector2.zero);
    }
    public IEnumerator Idle(float numberOfSeconds)
    {
        // Idle
        Move(Vector2.zero);

        yield return new WaitForSeconds(numberOfSeconds * idleTimeModifier);
    }

    public void PlayAttackAnimation(EnumCharacterAnimationStateName attackAnimation)
    {
        characterAnimator.ChangeAnimationState(attackAnimation);
        characterAnimator.SetWaitForAnimationToComplete(true);
    }

    public void FinishAttackAnimation()
    {
        characterAnimator.SetWaitForAnimationToComplete(false);
    }

    public Vector2 GetPlayerDirection()
    {
        // if the player x position is greater than the current enemy position
        if (playerTransform.position.x > transform.position.x)
            return Vector2.right;
        else
            return Vector2.left;
    }

    public void MoveTowardsPlayer(float speedModifier = 1)
    {
        float setSpeedModifier = baseSpeedModifier * speedModifier;

        Vector2 playerDirection = GetPlayerDirection();
        Move(playerDirection, setSpeedModifier);
    }


    #endregion
}