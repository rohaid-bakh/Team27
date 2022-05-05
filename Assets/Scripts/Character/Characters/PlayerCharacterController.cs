using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    
    #region Input functions
    void OnMove(InputValue value)
    {
        Move(value.Get<Vector2>());
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
        Attack(EnumCharacterAnimationStateName.Attack1);
    }

    void OnKick(InputValue value)
    {
        // second attack
        Attack(EnumCharacterAnimationStateName.Attack2);
    }

    void OnBlock(InputValue value)
    {
        if (value.isPressed)
        {
            Block();
        }
    }

    void OnDodge(InputValue value)
    {
        if (value.isPressed)
        {
            Dodge();
        }
        else
        {
            Move(Vector3.zero); // sets the player back to idle
        }
    }
    #endregion
}