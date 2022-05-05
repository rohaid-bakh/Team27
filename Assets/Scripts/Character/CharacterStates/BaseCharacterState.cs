using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

// Base Character State class with null / empty values for most fields (used to avoid code repetition in the other staes
// that will inherit from this class, since not all functions need to be implemented in each state
public class BaseCharacterState : ICharacterState
{
    public virtual EnumCharacterAnimationStateName? GetCharacterAnimationStateName() => null;

    public virtual EnumCharacterState GetState() => EnumCharacterState.Idling;

    public virtual void OnEnter(ICharacterContext context)
    {
        Debug.Log($"OnEnter: {GetState()}");
        context.PlayAnimation(GetCharacterAnimationStateName());
    }
    public virtual void OnUpdate(ICharacterContext context)
    {
    }

    public virtual void OnExit(ICharacterContext context)
    {
    }

    public virtual void Attack(ICharacterContext context, AttackNew attack)
    {
    }

    public virtual void Block(ICharacterContext context)
    {
    }

    public virtual void Dodge(ICharacterContext context)
    {
    }

    public virtual void TakeDamage(ICharacterContext context)
    {
    }

    public virtual void Idle(ICharacterContext context)
    {
    }

    public virtual void Jump(ICharacterContext context)
    {
    }

    public virtual void Walk(ICharacterContext context)
    {
    }
}
