using System.Collections;
using UnityEngine;

public class PlayerAttack1 : BaseAttack
{
    public override EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack1;
    public override EnumSoundName? GetSoundEffectName() => EnumSoundName.PlayerAttack1;
}