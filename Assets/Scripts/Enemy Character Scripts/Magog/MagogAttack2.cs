using System.Collections;
using UnityEngine;

// all attacks need to inherit from IAttack (BaseAttack inherits from IAttack)
public class MagogAttack2 : MonoBehaviour, IAttack
{
    public EnumCharacterAnimationStateName? GetAnimationStateName() => EnumCharacterAnimationStateName.Attack2;
    public EnumSoundName? GetSoundEffectName() => null; //todo

    public void Attack()
    {
        //todo
    }
}