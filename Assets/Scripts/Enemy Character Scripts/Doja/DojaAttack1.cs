using System.Collections;
using UnityEngine;

// This attack is the standard basic attack (will swipe at player)
public class DojaAttack1 : BaseAttack
{
    public override EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack1; 
    public override EnumSoundName? GetSoundEffectName() => null; // todo
}