using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TakingDamageCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.GettingHit;
    public override EnumCharacterState GetState() => EnumCharacterState.GettingHit;
    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
    }

    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

}
