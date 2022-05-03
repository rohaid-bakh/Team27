using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    // to identify the sound
    public EnumSound name;

    // audio clip to play
    public AudioClip clip;

    // default volume
    [Range(0f, 1f)]
    public float volume;

    // default pitch
    [Range(.1f, 3f)]
    public float pitch;

    // should the sound repeat
    public bool loop;

    // the audio source where the sound will play from 
    [HideInInspector]
    public AudioSource source;
}
