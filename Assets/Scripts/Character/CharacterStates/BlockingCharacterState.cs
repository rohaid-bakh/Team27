using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlockingCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Blocking;
    public override EnumCharacterState GetState() => EnumCharacterState.Blocking;

    public override void Walk(ICharacterContext context)
    {
        context.SetState(new WalkingCharacterState());
    }

    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

    public override void TakeDamage(ICharacterContext context)
    {
        // todo: take less damage if coming from block state
        context.SetState(new TakingDamageCharacterState());
    }

}
