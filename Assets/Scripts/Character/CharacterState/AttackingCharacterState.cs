﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AttackingCharacterState : BaseCharacterState
{
    IAttack Attack;
    public AttackingCharacterState(IAttack attack)
    {
        Attack = attack;
    }
    public override EnumCharacterState GetState() => EnumCharacterState.Attacking;
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => Attack.GetAnimationStateName();
    public override EnumSoundName? GetSoundEffectName() => Attack.GetSoundEffectName();

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        Attack.Attack();
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // wait for animation to finish
        if (!context.IsWaitingForAnimationToFinish())
        {
            if (context.IsGrounded() & context.IsMoving())
                context.SetState(new WalkingCharacterState());
            else if (context.IsGrounded() & !context.IsMoving())
                context.SetState(new IdlingCharacterState());
        }
    }

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        context.SetState(new TakingDamageCharacterState(damageAmount));
    }

    //win?
}