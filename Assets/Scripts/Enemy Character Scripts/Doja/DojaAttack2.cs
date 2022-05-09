using System.Collections;
using UnityEngine;

// This attack involves firing projectiles at the character
public class DojaAttack2 : BaseAttack
{
    public override EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public override EnumSoundName? GetSoundEffectName() => EnumSoundName.DojaClaw;
}