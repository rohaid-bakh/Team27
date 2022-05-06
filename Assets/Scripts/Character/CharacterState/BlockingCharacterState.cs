using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BlockingCharacterState : BaseCharacterState
{
    public override EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => EnumCharacterAnimationStateName.Blocking;
    public override EnumCharacterState GetState() => EnumCharacterState.Blocking;

    public override void TakeDamage(ICharacterContext context, int damageAmount)
    {
        // nothing happens because player is blocking
    }

}
