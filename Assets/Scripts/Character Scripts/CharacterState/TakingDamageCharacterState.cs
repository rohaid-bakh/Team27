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

    public override EnumCharacterState GetState() => EnumCharacterState.GettingHit;

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        DoTakeDamage(context);
    }

    public override void OnUpdate(ICharacterContext context)
    {
        // wait for animation to finish
        if (!context.IsWaitingForAnimationToFinish())
        {
            //Debug.Log($"Character Grounded: {context.IsGrounded()} IsMoving: {context.IsMovingLeftOrRight()}");
            if (context.IsGrounded() & context.IsMovingLeftOrRight())
            {
                context.SetState(new WalkingCharacterState());
            }
            else if (context.IsGrounded() & !context.IsMovingLeftOrRight())
            {
                context.SetState(new IdlingCharacterState());
            }
        }
    }

    private void DoTakeDamage(ICharacterContext context)
    {
        bool isCharacterDead = context.ApplyDamageToHealth(DamageAmount);
        if (isCharacterDead)
            context.SetState(new DeadCharacterState());
    }
}
