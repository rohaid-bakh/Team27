using System.Collections;
using UnityEngine;

// This attack is the standard basic attack (will swipe at player)
public class DojaAttack3 : MonoBehaviour, IAttack
{
    public EnumCharacterAnimationStateName? GetAnimationStateName() => null; // todo
    public EnumSoundName? GetSoundEffectName() => null; //todo

    public void Attack()
    {
        
    }
}