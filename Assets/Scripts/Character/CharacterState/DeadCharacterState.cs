using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DeadCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => null; //todo
    public override EnumCharacterState GetState() => EnumCharacterState.Dead;

    // can't transition to any other states when dead
}
