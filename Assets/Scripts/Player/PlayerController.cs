using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // movement
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] int jumpPower = 5;
    Vector2 moveInput;

    // ground check
    [SerializeField] Transform groundPoint;
    [SerializeField] LayerMask whatIsGround;

    // colliders / rigid body
    Rigidbody rigidBody;

    // animations
    PlayerAnimator playerAnimator;

    #region Awake, Start, Update
    void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
    }

    void FixedUpdate()
    {
        Move();
        SetAnimation();
    }
    #endregion

    #region Input functions
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            rigidBody.velocity += new Vector3(0f, jumpPower, 0f);
        }
    }

    void OnPunch(InputValue value)
    {
        if (!playerAnimator.waitingForAnimationToComplete)
        {
            playerAnimator.ChangeAnimationState(EnumPlayerAnimationState.Punching);
        }
    }

    void OnKick(InputValue value)
    {
        if (!playerAnimator.waitingForAnimationToComplete)
        {
            playerAnimator.ChangeAnimationState(EnumPlayerAnimationState.Kicking);
        }
    }
    #endregion

    #region Movement functions
    bool IsGrounded()
    {
        return Physics.Raycast(groundPoint.position, Vector3.down, .3f, whatIsGround);
    }

    void Move()
    {
        Vector3 playerVelocity = new Vector3(moveInput.x * moveSpeed, rigidBody.velocity.y, rigidBody.velocity.x);
        rigidBody.velocity = playerVelocity;

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

    void SetAnimation()
    {
        EnumPlayerAnimationState state;

        // check if any of the animations are currently playing that can't be interuppted (for example, attack animations) 
        if (!playerAnimator.waitingForAnimationToComplete)
        {
            Vector2 playerVelocity = rigidBody.velocity;

            bool isTouchingGround = IsGrounded();
            bool isWalking = isTouchingGround && Mathf.Abs(playerVelocity.x) > Mathf.Epsilon;
            bool isJumping = !isTouchingGround && Mathf.Abs(playerVelocity.y) > Mathf.Epsilon;

            // change animation state based on whether the player is jumping, walking, etc.
            if (isJumping)
                state = EnumPlayerAnimationState.Jumping;
            else if (isWalking)
                state = EnumPlayerAnimationState.Walking;
            else // otherwise idle
                state = EnumPlayerAnimationState.Idling;

            playerAnimator.ChangeAnimationState(state);
        }
    }
    #endregion

}