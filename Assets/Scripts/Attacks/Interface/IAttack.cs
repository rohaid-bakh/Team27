using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IAttack
{
    public void Attack();
    public EnumCharacterAnimationStateName? GetAnimationStateName();
    public EnumSoundName? GetSoundEffectName();
}
