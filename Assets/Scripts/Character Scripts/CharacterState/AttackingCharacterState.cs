using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackingCharacterState : BaseCharacterState
{
    IAttack CurrentAttack;
    public AttackingCharacterState(IAttack attack)
    {
        CurrentAttack = attack;
    }
    public override EnumCharacterState GetState() => EnumCharacterState.Attacking;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => CurrentAttack.GetAnimationStateName();
    public override EnumSoundName? GetSoundEffectName() => CurrentAttack.GetSoundEffectName();

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        context.PlayAnimation(GetCharacterAnimationStateName());
        CurrentAttack.Attack();
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // wait for animation to finish
        if (!context.IsWaitingForAnimationToFinish())
        {
            if (context.IsGrounded() & context.IsMovingLeftOrRight())
                context.SetState(new WalkingCharacterState());
            else if (context.IsGrounded() & !context.IsMovingLeftOrRight())
                context.SetState(new IdlingCharacterState());
        }
    }

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        context.SetState(new TakingDamageCharacterState(damageAmount));
    }

    //win?
}
