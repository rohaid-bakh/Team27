using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// used to name/identify the various sounds such as music tracks or sound effects
public enum EnumSoundName
{
    MainTheme,
    SoundVolumeChange,
    Jump,
    PlayerAttack1,
    PlayerAttack2,
    MagogAttack,
    MagogProjectile
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
    GettingHit,
    Die
}

// used to identify the player state
public enum EnumCharacterState
{
    Idling,
    Walking,
    Jumping,
    Attacking,
    Blocking,
    Dodging,
    GettingHit,
    Dead
}