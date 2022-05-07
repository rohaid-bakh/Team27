using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    [SerializeField] PlayerAttack1 attack1;

    private void Start()
    {
        // for player we want to avoid the collision with the boundary collidesr (the player can fall of the map)
        // boundaries are for the enemies
        int ignoreBoundariesLayer = LayerMask.NameToLayer("Boundaries");
        Collider[] colliders = FindObjectsOfType<Collider>();
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.layer == ignoreBoundariesLayer)
            {
                Physics.IgnoreCollision(characterCollider, collider, ignore: true);
            }
        }
    }

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