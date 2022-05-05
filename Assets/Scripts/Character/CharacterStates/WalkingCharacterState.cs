using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WalkingCharacterState : BaseCharacterState
{
    public override EnumCharacterState GetState() => EnumCharacterState.Walking;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Walking;

    public override void OnUpdate(ICharacterContext context)
    {
        base.OnUpdate(context);

        // if player is grounded and stops moving, switch to idle
        if (context.IsGrounded() && !context.IsMoving())
            context.SetState(new IdlingCharacterState());
    }

    public override void Jump(ICharacterContext context)
    {
        context.SetState(new JumpingCharacterState());
    }

    public override void Attack(ICharacterContext context, AttackNew attack)
    {
        context.SetState(new AttackingCharacterState(attack));
    }

    public override void Block(ICharacterContext context)
    {
        context.SetState(new BlockingCharacterState());
    }

    public override void Dodge(ICharacterContext context)
    {
        context.SetState(new DodgingCharacterState());
    }

    public override void TakeDamage(ICharacterContext context)
    {
        context.SetState(new TakingDamageCharacterState());
    }
}
