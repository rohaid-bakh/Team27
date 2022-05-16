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
    PlayerAttackHit,
    PlayerTakeDamage,
    MagogAttack,
    MagogProjectile,
    MagogCharge,
    BorealProjectile,
    DojaBite,
    DojaClaw,
    PowerUpAppear,
    PowerUpPickUp,
    PowerUpDebuff,
    PowerUpReady,
    MagogFightTheme,
    BorealFightTheme,
    IntroDialog1,
    IntroDialog2,
    IntroDialog3,
    IntroDialog4,
    IntroDialog5,
    IntroDialog6,
    OutroDialog1,
    OutroDialog2,
    OutroDialog3,
    OutroDialog4,
    OutroDialog5,
    OutroDialog6
}

// tag names for the power ups
public enum EnumPowerUpTag
{
    Health = 0,
    Speed = 1,
    Strength = 2
}

// note: these names must match the states in the animator for the player / enemy
public enum EnumCharacterAnimationStateName
{
    Idling,
    Walking,
    Jumping,
    Attack1,
    Attack2,
    Attack3,
    Blocking,
    Dodging,
    GettingHit,
    Die,
    EnterRage
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