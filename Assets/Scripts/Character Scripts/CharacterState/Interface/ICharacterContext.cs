using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ICharacterContext
{
    void SetState(ICharacterState newState);
    void PlayAnimation(EnumCharacterAnimationStateName? animationStateName);
    void PlaySoundEffect(EnumSoundName? soundEffectName);
    void SetCanMoveBool(bool canMove);
    bool ApplyDamageToHealth(int damageAmont);
    float GetJumpForce();
    void AddToJumpVelocity(Vector3 force);
    bool IsGrounded();
    bool IsMovingLeftOrRight();
    bool IsJumping();
    bool IsWaitingForAnimationToFinish();
}
