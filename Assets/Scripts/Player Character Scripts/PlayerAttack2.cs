using System.Collections;
using UnityEngine;

public class PlayerAttack2 : BaseAttack
{
    public override EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public override EnumSoundName? GetSoundEffectName() => EnumSoundName.PlayerAttack2;
}