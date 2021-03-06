using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DeadCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Die;
    public override EnumCharacterState GetState() => EnumCharacterState.Dead;

    public override void OnEnter(ICharacterContext context)
    {
        base.OnEnter(context);
        context.SetCanMoveBool(false); // can't move
        Debug.Log("Character in dead state.");

        // destroy shadow
        context.DestroyShadow();
    }

    // can't transition to any other states when dead
}
