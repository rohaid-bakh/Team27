using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DodgingCharacterState : BaseCharacterState
{
    public override EnumCharacterState GetState() => EnumCharacterState.Dodging;
    public override void Idle(ICharacterContext context)
    {
        context.SetState(new IdlingCharacterState());
    }

    public override void TakeDamage(ICharacterContext context)
    {
        context.SetState(new TakingDamageCharacterState());
    }
}
