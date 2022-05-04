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

    // todo: maybe change OnOunch & OnKick to more generic "Attack1" and "Attack2"?
    void OnPunch(InputValue value)
    {
        // todo: replace with real attack. Maybe we have a seralizable field with a list of possible attacks?
        Attack(new Attack());
    }

    void OnKick(InputValue value)
    {
        // todo: replace with real attack.  Maybe we have a seralizable field with a list of possible attacks?
        Attack(new Attack());
    }
    #endregion
}