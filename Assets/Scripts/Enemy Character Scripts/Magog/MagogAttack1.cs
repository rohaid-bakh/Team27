using System.Collections;
using UnityEngine;

// all attacks need to inherit from IAttack (BaseAttack inherits from IAttack)
public class MagogAttack1 : BaseAttack
{
    public override EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack1;
    public override EnumSoundName? GetSoundEffectName() => EnumSoundName.PlayerAttack1; // just using the player attack sound for now
}