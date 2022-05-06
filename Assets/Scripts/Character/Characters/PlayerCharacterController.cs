using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    [SerializeField] AttackNew attack1;
    [SerializeField] AttackNew attack2;

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

    void OnPunch(InputValue value)
    {
        // first attack
        Attack(attack1);
    }

    void OnKick(InputValue value)
    {
        // second attack
        Attack(attack2);
    }
    #endregion
}