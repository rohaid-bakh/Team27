using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TakingDamageCharacterState : BaseCharacterState
{
    int DamageAmount;
    public TakingDamageCharacterState(int damageAmount)
    {
        DamageAmount = damageAmount;
    }

    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => null; //todo
    public override EnumCharacterState GetState() => EnumCharacterState.GettingHit;

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        DoTakeDamage(context);
    }

    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

    public override void Walk(ICharacterContext context)
    {
        context.SetState(new WalkingCharacterState());
    }

    private void DoTakeDamage(ICharacterContext context)
    {
        bool isCharacterDead = context.ApplyDamageToHealth(DamageAmount);
        if (isCharacterDead)
            context.SetState(new DeadCharacterState());
        else // todo maybe transition to different state
            context.SetState(new IdlingCharacterState());
    }
}
