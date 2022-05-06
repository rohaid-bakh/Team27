using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAttack
{
    // the actual attack function. You can put whatever you want in here that will take place during the attack. 
    // the CharacterMonoBehaviour class will execute this function at the same time as the animation. 
    // note: If you just want a basic simple attack, you can inherit from BaseAttack instead.
    public void Attack();

    // the name of the attack animation (likely Attack1 or Attack2). Can be set to return null also (no animation occurs)
    public EnumCharacterAnimationStateName? GetAnimationStateName();

    // the sound effect for the attack(). Can be set to return null (no sound effect occurs)
    public EnumSoundName? GetSoundEffectName();
}
