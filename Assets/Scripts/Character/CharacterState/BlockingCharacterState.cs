using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class BlockingCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Blocking;
    public override EnumCharacterState GetState() => EnumCharacterState.Blocking;

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        context.SetCanMoveBool(false); // pause moving when blocking
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // wait for animation to finish
        if (!context.IsWaitingForAnimationToFinish())
        {
            context.SetCanMoveBool(true); // resume moving
            if (context.IsGrounded() & context.IsMoving())
                context.SetState(new WalkingCharacterState());
            else if (context.IsGrounded() & !context.IsMoving())
                context.SetState(new IdlingCharacterState());
        }
    }

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        // nothing happens because player is blocking (maybe add some sort of block effect animation to signal block)
    }

}
