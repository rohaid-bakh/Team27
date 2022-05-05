using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnumSoundName
{
    MainTheme,
    SoundVolumeChange
}

// note: these names must match the states in the animator for the player / enemy
public enum EnumCharacterAnimationStateName
{
    Idling,
    Walking,
    Jumping,
    Attack1,
    Attack2,
    Blocking,
    Dodging,
    GettingHit
}

public enum EnumCharacterState
{
    Idling,
    Walking,
    Jumping,
    Attacking,
    Blocking,
    Dodging,
    GettingHit
}