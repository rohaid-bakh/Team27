using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class WalkingCharacterState : BaseCharacterState
{
    public override EnumCharacterState GetState() => EnumCharacterState.Walking;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Walking;

    public override void OnUpdate(ICharacterContext context)
    {
        base.OnUpdate(context);

        // if player is grounded and stops moving, switch to idle
        if (context.IsGrounded() && !context.IsMovingLeftOrRight())
            context.SetState(new IdlingCharacterState());
    }

    public override void Jump(ICharacterContext context)
    {
        context.SetState(new JumpingCharacterState());
    }

    public override void Attack(ICharacterContext context, IAttack attack)
    {
        context.SetState(new AttackingCharacterState(attack));
    }
}
