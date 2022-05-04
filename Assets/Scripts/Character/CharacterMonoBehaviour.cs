using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Basic character monobehaviour. Can be used for both player + enemy types.
/// </summary>
public class CharacterMonoBehaviour : MonoBehaviour
{
    // movement
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int jumpPower = 5;
    [HideInInspector] public Vector2 moveInput;

    // ground check
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask whatIsGround;

    // attacks
    [HideInInspector] List<AttackNew> attacks;

    // rigid body
    [HideInInspector] public Rigidbody rigidBody;

    // animations
    public CharacterAnimator characterAnimator { get; private set; }

    #region Awake, Start, Update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        attacks = GetComponentsInChildren<AttackNew>().ToList();
    }

    void FixedUpdate()
    {
        Move();
        SetAnimation();
    }
    #endregion

    #region Actions
    public void Move()
    {
        Vector3 characterVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.velocity.y, rigidBody.velocity.x);
        rigidBody.velocity = characterVelocity;

        FlipSprite();
    }

    public void Jump()
    {
        rigidBody.velocity += new Vector3(0f, jumpPower, 0f);
    }

    // performs the attack from the list of attacks 
    public void Attack(EnumCharacterAnimationState attackAnimationState)
    {
        AttackNew attack = attacks?.FirstOrDefault(x => x.animationState == attackAnimationState);
        if (attack != null)
        {
            // animation
            characterAnimator.ChangeAnimationState(attackAnimationState);

            // attack
            attack.Hit();
        }
        else
        {
            Debug.Log($"Attack doesn't exist for {attackAnimationState.ToString()}");
        }
    }

    #endregion 

    #region Helper functions
    public bool IsGrounded()
    {
        return Physics.Raycast(groundPoint.position, Vector3.down, .3f, whatIsGround);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is techincally 0

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }
    }

    void SetAnimation()
    {
        EnumCharacterAnimationState state;

        // check if any of the animations are currently playing that can't be interuppted (for example, attack animations) 
        if (characterAnimator != null && !characterAnimator.waitingForAnimationToComplete)
        {
            Vector2 playerVelocity = rigidBody.velocity;

            bool isTouchingGround = IsGrounded();
            bool isWalking = isTouchingGround && Mathf.Abs(playerVelocity.x) > Mathf.Epsilon;
            bool isJumping = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon;

            // change animation state based on whether the player is jumping, walking, etc.
            if (isJumping)
                state = EnumCharacterAnimationState.Jumping;
            else if (isWalking)
                state = EnumCharacterAnimationState.Walking;
            else // otherwise idle
                state = EnumCharacterAnimationState.Idling;

            characterAnimator.ChangeAnimationState(state);
        }
    }
    #endregion

}