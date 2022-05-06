using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface ICharacterState
{
    EnumCharacterState GetState();
    EnumCharacterAnimationStateName? GetCharacterAnimationStateName();
    EnumSoundName? GetSoundEffectName();
    void OnEnter(ICharacterContext context);
    void OnUpdate(ICharacterContext context);
    void OnExit(ICharacterContext context);
    void Idle(ICharacterContext context);
    void Walk(ICharacterContext context);
    void Jump(ICharacterContext context);
    void Attack(ICharacterContext context, AttackNew attack);
    void Block(ICharacterContext context);
    void Dodge(ICharacterContext context);
    void TakeDamage(ICharacterContext context, int damageAmount);
}