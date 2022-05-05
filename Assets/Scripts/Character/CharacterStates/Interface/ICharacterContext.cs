using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ICharacterContext
{
    void SetState(ICharacterState newState);
    void PlayAnimation(EnumCharacterAnimationStateName? animationStateName);
    float GetJumpForce();
    void AddForceToVelocity(Vector3 force);
    bool IsGrounded();
    bool IsMoving();
    bool IsJumping();
    bool IsWaitingForAnimationToFinish();
}
