using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum EnumSound
{
    MainTheme,
    SoundVolumeChange
}

// note: these names must match the states in the animator for the player / enemy
public enum EnumCharacterAnimationState
{
    Idling,
    Walking,
    Jumping,
    Attack1,
    Attack2
}