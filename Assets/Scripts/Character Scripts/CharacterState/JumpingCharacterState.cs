using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

public class JumpingCharacterState : BaseCharacterState
{
    public override EnumCharacterState GetState() => EnumCharacterState.Jumping;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Jumping;

    public override EnumSoundName? GetSoundEffectName() => EnumSoundName.Jump;

    bool hasStartedJump = false;

    public override void OnEnter(ICharacterContext context)
    {
        DoJump(context);
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // check if player has landed, then resume walking or idling state
        if (hasStartedJump == true && context.IsGrounded() && !context.IsJumping())
        {
            if (context.IsMovingLeftOrRight())
                context.SetState(new WalkingCharacterState());
            else
                context.SetState(new IdlingCharacterState());
        }
    }

    // no attack when jumping
    public override void Attack(ICharacterContext context, IAttack attack)
    {
        context.SetState(new AttackingCharacterState(attack));
    }

    private void DoJump(ICharacterContext context)
    {
        // check if grounded first, then jump
        if (context.IsGrounded())
        {
            // plays the animations
            base.OnEnter(context);

            // does the jump
            Vector3 jumpForce = new Vector3(0f, context.GetJumpForce(), 0f);
            context.AddToJumpVelocity(jumpForce);
        }

        hasStartedJump = true;
    }
}
