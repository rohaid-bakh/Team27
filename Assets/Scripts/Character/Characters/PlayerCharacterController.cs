using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    
    #region Input functions
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && IsGrounded())
        {
            Jump();
        }
    }

    void OnPunch(InputValue value)
    {
        // first attack
        Attack(EnumCharacterAnimationState.Attack1);
    }

    void OnKick(InputValue value)
    {
        // second attack
        Attack(EnumCharacterAnimationState.Attack2);
    }
    #endregion
}