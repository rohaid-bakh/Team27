using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

    public override void Attack(ICharacterContext context, IAttack attack)
    {
        context.SetState(new AttackingCharacterState(attack));
    }

}
