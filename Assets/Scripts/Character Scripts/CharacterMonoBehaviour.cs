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
    [SerializeField] public float moveSpeed = 2f;
    [SerializeField] public float jumpPower = 5;
    [SerializeField] float fallModifier = 2f; //how fast to fall after jumping
    Vector2 moveInput = new Vector2();

    // ground check
    [SerializeField] Transform groundPoint; // set a transform point on the character where the ground is
    [SerializeField] float distanceToGround = 0.1f; // how close should the groundPoint be to the ground, to be considered "grounded"
    [SerializeField] LayerMask whatIsGround; // set the ground layer mask
    
    // rigid body/collider
    public Rigidbody rigidBody { get; private set; } // character needs a rigid body attached
    public CapsuleCollider characterCollider { get; private set; }

    // animations
    public CharacterAnimator characterAnimator { get; private set; } // character needs a characterAnimator attached

    // health
    public Health characterHealth { get; private set; } // character needs a health component attached

    // text mesh (for testing purposes, placed on top of character to view states)
    TextMesh testingTextMesh = null;

    ICharacterState currentState = new IdlingCharacterState();

    private bool canMove = true;

    #region Awake, Start, Update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        characterAnimator = GetComponentInChildren<CharacterAnimator>();
        characterHealth = GetComponent<Health>();
        testingTextMesh = GetComponentInChildren<TextMesh>();
        characterCollider = GetComponent<CapsuleCollider>();
    }

    void FixedUpdate()
    {
        Move();
        HandleFallVelocity();
        currentState.OnUpdate(this);
        SetAnimation();
        UpdateTeshMeshWithCharacterState(); // for testing. Can be removed when finished game
    }
    #endregion

    #region Character Actions

    /// <summary>
    /// Used to move the character in the provided direction (left, right, or no direction)
    /// </summary>
    /// <param name="newMoveInput">Ex. Vector2.right, Vector2.left, Vector2.zero</param>
    /// <param name="speedModifier">If you want to increase speed (ex. 1.25) or decrease speed (ex. 0.75). 1 by default</param>
    public virtual void Move(Vector2 newMoveInput, float speedModifier = 1) 
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
    public virtual void Attack(IAttack attack)
    {
        currentState.Attack(this, attack);
    }

    /// <summary>
    /// To make the character jump
    /// </summary>
    public virtual void Jump() => currentState.Jump(this);

    /// <summary>
    /// When the character takes damage
    /// </summary>
    /// <param name="damageAmount">Amount of damage to be applied</param>
    public virtual void TakeDamage(int damageAmount) 
    {
        bool isCharacterDead = ApplyDamageToHealth(damageAmount);
        if (isCharacterDead)
            currentState.Die(this);
    } 

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
        return Mathf.Abs(rigidBody.velocity.y) > Mathf.Epsilon;
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

    // used to display character state (for testing purposes)
    void UpdateTeshMeshWithCharacterState()
    {
        if(testingTextMesh != null)
        {
            testingTextMesh.text = currentState.GetState().ToString();
        }
    }

    // temporarily switch layers to avoid collision with opponent
    public IEnumerator IgnoreCollisionTemporarily(Collider opponentCollider, float duration = 1f)
    {
        Debug.Log("Turning collisions off");

        Physics.IgnoreCollision(characterCollider, opponentCollider, ignore:true);

        yield return new WaitForSeconds(duration);

        Physics.IgnoreCollision(characterCollider, opponentCollider, ignore:false);
    }

    public bool IsWaitingForAnimationToFinish()
    {
        return characterAnimator.waitingForAnimationToComplete;
    }

    public float GetJumpForce()
    {
        return jumpPower;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    public float GetMoveInputX()
    {
        return moveInput.x;
    }

    public void SetCanMoveBool(bool canMove)
    {
        this.canMove = canMove;
    }

    public bool GetCanMoveBool()
    {
        return canMove;
    }

    public void AddToJumpVelocity(Vector3 force)
    {
        rigidBody.velocity += new Vector3(0, 0.1f, 0); // add a small amount of velocity first so IsJumping() returns true
        rigidBody.AddForce(force, ForceMode.Impulse); // add force
    }

    public void ApplyForceToVelocity(Vector3 force)
    {
        rigidBody.AddForce(force, ForceMode.Impulse);
    }

    public int GetMaxHealthStat()
    {
        return characterHealth.stat.health;
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
            characterAnimator?.ChangeAnimationState((EnumCharacterAnimationStateName)animationStateName);
    }

    // used to play sound effect based on sound effect name
    public void PlaySoundEffect(EnumSoundName? soundEffectName)
    {
        if (soundEffectName != null)
            AudioManager.instance?.PlaySoundEffect((EnumSoundName)soundEffectName);
    }

    // used to stop sound effect based on sound effect name
    public void StopSoundEffect(EnumSoundName? soundEffectName)
    {
        if (soundEffectName != null)
            AudioManager.instance?.StopSoundEffect((EnumSoundName)soundEffectName);
    }

    public virtual void Move()
    {
        if (canMove)
        {
            Vector3 characterVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
            rigidBody.velocity = characterVelocity;

            if (Mathf.Abs(moveInput.x) != 0)
            {
                FlipSprite();
            }
        }
    }

    public virtual void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidBody.velocity.x) > Mathf.Epsilon; // Mathf.Epsilon is techincally 0

        if (playerHasHorizontalSpeed)
        {
            FlipSprite(rigidBody.velocity.x);
        }
    }

    public virtual void FlipSprite(float direction)
    {
        transform.localScale = new Vector2(Mathf.Sign(direction), 1f);
    }

    public virtual void HandleFallVelocity()
    {
        // if falling
        if(rigidBody.velocity.y != 0 && rigidBody.velocity.y < 0.5)
        {
            // falls faster
            rigidBody.velocity += (Vector3.up * Physics.gravity.y * fallModifier * Time.deltaTime);
        }
    }

    void SetAnimation()
    {
        EnumCharacterAnimationStateName state;

        Vector2 playerVelocity = rigidBody.velocity;

        bool isDead = IsDead();
        bool isTouchingGround = IsGrounded();
        bool isMoving = isTouchingGround && Mathf.Abs(playerVelocity.x) > Mathf.Epsilon;
        bool isJumping = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon;

        if (!IsWaitingForAnimationToFinish())
        {
            if (isDead)
                state = EnumCharacterAnimationStateName.Die;
            else if (isJumping)
                state = EnumCharacterAnimationStateName.Jumping;
            else if (isMoving)
                state = EnumCharacterAnimationStateName.Walking;
            else
                state = EnumCharacterAnimationStateName.Idling;

            PlayAnimation(state);
        }
    }

    public void DestroyShadow() 
    {
        Shadow shadow = GetComponent<Shadow>();
        if(shadow != null)
        {
            Destroy(shadow.shadow);
        }
    }

    #endregion

}