using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TakingDamageCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => null; //todo
    public override EnumCharacterState GetState() => EnumCharacterState.GettingHit;
    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

    public override void Walk(ICharacterContext context)
    {
        context.SetState(new WalkingCharacterState());
    }

}
