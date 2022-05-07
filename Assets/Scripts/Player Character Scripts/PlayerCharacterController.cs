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
        if(PauseMenu.GamePaused != true)
            Move(value.Get<Vector2>());
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            if (PauseMenu.GamePaused != true)
                Jump();
        }
    }

    void OnAttack1(InputValue value)
    {
        if (PauseMenu.GamePaused != true)
            // first attack
            Attack(attack1);
    }
    #endregion
}