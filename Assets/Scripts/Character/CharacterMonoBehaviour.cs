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

    // health
    public Health characterHealth { get; private set; }

    ICharacterState currentState = new IdlingCharacterState();

    #region Awake, Start, Update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        attacks = GetComponentsInChildren<AttackNew>().ToList();
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
    /// <param name="direction">Ex. Vector2.right, Vector2.left, Vector2.zero</param>
    public void Move(Vector2 direction) 
    {
        moveInput = direction;
        if (direction == Vector2.zero)
            currentState.Idle(this);
        else
            currentState.Walk(this);
    }

    /// <summary>
    /// Used to make an attack.
    /// </summary>
    /// <param name="attack"></param>
    public void Attack(AttackNew attack)
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
    public void TakeDamage() => currentState.TakeDamage(this); //todo, need to set up animation + health check

    #endregion

    #region Character Checks (IsBlocking, IsAttacking, etc.)
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
        return Physics.Raycast(groundPoint.position, Vector3.down, .3f, whatIsGround);
    }

    public bool IsMoving()
    {
        return Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon;
    }

    public bool IsJumping()
    {
        return Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
    }
    #endregion

    #region Helper functions
    public bool IsWaitingForAnimationToFinish()
    {
        return characterAnimator.waitingForAnimationToComplete;
    }

    public float GetJumpForce()
    {
        return jumpPower;
    }

    public void AddForceToVelocity(Vector3 force)
    {
        rigidBody.velocity += force;
    }
    
    // performs the attack from the list of attacks 
    public void Attack(EnumCharacterAnimationStateName attackAnimationState)
    {
        AttackNew attack = attacks?.FirstOrDefault(x => x.animationState == attackAnimationState);
        if (attack != null)
        {
            Attack(attack);
        }
        else
        {
            Debug.Log($"Attack doesn't exist for {attackAnimationState.ToString()}");
        }
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

    void Move()
    {
        Vector3 characterVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.velocity.y, rigidBody.velocity.x);
        rigidBody.velocity = characterVelocity;

        FlipSprite();
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