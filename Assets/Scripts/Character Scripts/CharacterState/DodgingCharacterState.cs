using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DodgingCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Blocking;
    public override EnumCharacterState GetState() => EnumCharacterState.Dodging;
    public override void OnEnter(ICharacterContext context)
    {
        DoDodge(context);
    }

    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        context.SetState(new TakingDamageCharacterState(damageAmount));
    }

    private void DoDodge(ICharacterContext context)
    {
        
    }
}
