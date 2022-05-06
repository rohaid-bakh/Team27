using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Basic character monobehaviour. Can be used for both player + enemy types.
/// </summary>
public class CharacterMonoBehaviour : MonoBehaviour, ICharacterContext
{
    // movement
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] int jumpPower = 5;
    Vector2 moveInput;

    // ground check
    [SerializeField] Transform groundPoint; // set a transform point on the character where the ground is
    [SerializeField] float distanceToGround = 0.1f; // how close should the groundPoint be to the ground, to be considered "grounded"
    [SerializeField] LayerMask whatIsGround; // set the ground layer mask
    
    // rigid body
    Rigidbody rigidBody; // character needs a rigid body attached

    // animations
    CharacterAnimator characterAnimator; // character needs a characterAnimator attached

    // health
    Health characterHealth; // character needs a health component attached

    ICharacterState currentState = new IdlingCharacterState();

    private bool canMove = true;

    #region Awake, Start, Update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterHealth = GetComponent<Health>();
    }

    void FixedUpdate()
    {
        Move();
        currentState.OnUpdate(this);
    }
    #endregion

    #region Character Actions

    /// <summary>
    /// Used to move the character in the provided direction (left, right, or no direction)
    /// </summary>
    /// <param name="newMoveInput">Ex. Vector2.right, Vector2.left, Vector2.zero</param>
    /// <param name="speedModifier">If you want to increase speed (ex. 1.25) or decrease speed (ex. 0.75). 1 by default</param>
    public void Move(Vector2 newMoveInput, float speedModifier = 1) 
    {
        moveInput = newMoveInput * speedModifier;
        if (newMoveInput == Vector2.zero)
            currentState.Idle(this);
        else
            currentState.Walk(this);
    }

    /// <summary>
    /// Used to make an attack.
    /// </summary>
    /// <param name="attack">The attack you want to perform.</param>
    public void Attack(IAttack attack)
    {
        currentState.Attack(this, attack);
    }

    /// <summary>
    /// To make the character jump
    /// </summary>
    public void Jump() => currentState.Jump(this);

    /// <summary>
    /// To make the character block
    /// </summary>
    public void Block() => currentState.Block(this); 

    /// <summary>
    /// To make the character dodge
    /// </summary>
    public void Dodge() => currentState.Dodge(this); //todo - maybe

    /// <summary>
    /// When the character takes damage
    /// </summary>
    /// <param name="damageAmount">Amount of damage to be applied</param>
    public void TakeDamage(int damageAmount) => currentState.TakeDamage(this, damageAmount); //todo, need to set up animation + health check

    #endregion

    #region Character Checks (IsBlocking, IsAttacking, etc.)
    public EnumCharacterState GetState()
    {
        return currentState.GetState();
    }

    public bool IsDead()
    {
        if (currentState.GetState() == EnumCharacterState.Dead)
            return true;
        return false;
    }

    public bool IsAttacking()
    {
        if (currentState.GetState() == EnumCharacterState.Attacking)
            return true;
        return false;
    }

    public bool IsBlocking()
    {
        if (currentState.GetState() == EnumCharacterState.Blocking)
            return true;
        return false;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(groundPoint.position, Vector3.down, distanceToGround, whatIsGround);
    }

    public bool IsMovingLeftOrRight()
    {
        return Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
    }

    public bool IsJumping()
    {
        return Mathf.Abs(rigidBody.velocity.y) > 0.2;
    }
    #endregion

    // these are helper functions that are just used for this class. If you inherit the class you can ignore these
    #region Helper functions

    // this function will draw a sphere in the scene view to see if player is grounded (if touches the ground layer, then it is considered grounded)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(groundPoint.position, distanceToGround);
    }

    public bool IsWaitingForAnimationToFinish()
    {
        return characterAnimator.waitingForAnimationToComplete;
    }

    public float GetJumpForce()
    {
        return jumpPower;
    }

    public void SetCanMoveBool(bool canMove)
    {
        this.canMove = canMove;
    }

    public void AddForceToVelocity(Vector3 force)
    {
        rigidBody.velocity += force;
    }
    
    public bool ApplyDamageToHealth(int damageAmont)
    {
        return characterHealth.TakeDamage(damageAmont);
    }

    /// used to change the character state and animations
    public void SetState(ICharacterState newState)
    {
        // on exit for current state
        currentState.OnExit(this);

        // set new state
        currentState = newState;

        // on enter
        currentState.OnEnter(this);
    }

    // used to play animation based on animation state name
    public void PlayAnimation(EnumCharacterAnimationStateName? animationStateName)
    {
        if(animationStateName != null)
            characterAnimator.ChangeAnimationState((EnumCharacterAnimationStateName)animationStateName);
    }

    // used to play sound effect based on sound effect name
    public void PlaySoundEffect(EnumSoundName? soundEffectName)
    {
        if (soundEffectName != null)
            AudioManager.instance.PlaySoundEffect((EnumSoundName)soundEffectName);
    }

    void Move()
    {
        if (canMove)
        {
            Vector3 characterVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.velocity.y, rigidBody.velocity.x);
            rigidBody.velocity = characterVelocity;

            FlipSprite();
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is techincally 0

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rigidBody.velocity.x), 1f);
        }
    }
    #endregion

}