using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class IdlingCharacterState : BaseCharacterState
{
    public override EnumCharacterState GetState() => EnumCharacterState.Idling;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Idling;

    public override void Walk(ICharacterContext context)
    {
        context.SetState(new WalkingCharacterState());
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

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        context.SetState(new TakingDamageCharacterState(damageAmount));
    }
}
