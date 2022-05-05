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

    public override void OnEnter(ICharacterContext context)
    {
        DoJump(context);
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // if touching ground & no longer jumping, set state
        if (context.IsGrounded() && !context.IsJumping())
        {
            if (context.IsMoving())
                context.SetState(new WalkingCharacterState());
            else
                context.SetState(new IdlingCharacterState());
        }
    }

    public override void Attack(ICharacterContext context, AttackNew attack)
    {
        context.SetState(new AttackingCharacterState(attack));
    }

    public override void TakeDamage(ICharacterContext context)
    {
        context.SetState(new TakingDamageCharacterState());
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
            context.AddForceToVelocity(jumpForce);
        }
    }
}
