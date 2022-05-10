using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterMonoBehaviour
{
    [SerializeField] PlayerAttack1 attack1;

    //attck rate / cool down
    [SerializeField] float attackCoolDownTime = 2f;
    float attackCoolDownTimer = 0f;

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            moveSpeed = value;
        }
    }

    public float JumpPower
    {
        get
        {
            return jumpPower;
        }
        set
        {
            jumpPower = value;
        }
    }

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

    private void Update()
    {
        // attack cool down timer
        attackCoolDownTimer -= Time.deltaTime;
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
        {
            // check if player can attack (based on attack rate/time since last attack)
            if(attackCoolDownTimer <= 0)
            {
                Attack(attack1);
                attackCoolDownTimer = attackCoolDownTime;
            }
        }
    }
    #endregion

    #region overrise
    public override void TakeDamage(int damageAmount)
    {
        base.TakeDamage(damageAmount);

        // sound effect
        PlaySoundEffect(EnumSoundName.PlayerTakeDamage);
    }

    #endregion
}