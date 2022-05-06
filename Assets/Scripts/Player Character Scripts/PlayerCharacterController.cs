using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    [SerializeField] PlayerAttack1 attack1;
    [SerializeField] PlayerAttack2 attack2;

    #region Input functions
    void OnMove(InputValue value)
    {
        Move(value.Get<Vector2>());
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            Jump();
        }
    }

    void OnBlock(InputValue value)
    {
        if (value.isPressed)
        {
            Block();
        }
    }

    void OnAttack1(InputValue value)
    {
        // first attack
        Attack(attack1);
    }

    void OnAttack2(InputValue value)
    {
        // second attack
        Attack(attack2);
    }
    #endregion
}