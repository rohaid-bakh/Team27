using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<Sound> soundEffects;
    [SerializeField] private List<Sound> musicTracks;

    private float musicVolume = 1f;
    private float soundEffectVolume = 1f;

    public static AudioManager instance;

    // Use this for initialization
    private void Awake()
    {
        // delete other instances of the audio manager
        ManageSingleton();

        // create an audio source for each music track
        InitializeAudioSources(musicTracks);

        // create an audio source for each sound effect 
        InitializeAudioSources(soundEffects);
    }

    private void Start()
    {
        // by default, play the main theme if nothing is playing on start
        Sound musicTrack = musicTracks.FirstOrDefault();
        if (musicTrack != null && !musicTrack.source.isPlaying)
        {
            PlayMusicTrack(EnumSoundName.MainTheme);
        }
    }

    #region public functions
    /// <summary>
    /// Used to play a sound effect from the list of sound effects in the audio manager.
    /// </summary>
    /// <param name="soundName"></param>
    public void PlaySoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            soundEffect.source.Play();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to play a music track from the list of tracks in the audio manager.
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayMusicTrack(EnumSoundName soundName)
    {
        Sound musicTrack = musicTracks.FirstOrDefault(s => s.soundName == soundName);
        if (musicTrack != null)
        {
            musicTrack.source.Play();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }

    /// <summary>
    /// Used to update the volume for all of the music tracks.
    /// </summary>
    /// <param name="volume">A value between 0..1</param>
    public void UpdateMusicVolume(float volume)
    {
        musicVolume = volume;
        UpdateVolume(musicVolume, musicTracks);
    }

    /// <summary>
    /// Used to update the volume for all of the sound effects.
    /// </summary>
    /// <param name="volume">A value between 0..1</param>
    public void UpdateSoundEffectVolume(float volume, bool playSoundEffect)
    {
        soundEffectVolume = volume;
        UpdateVolume(soundEffectVolume, soundEffects);

        // play sound effect
        if(playSoundEffect)
            PlaySoundEffect(EnumSoundName.SoundVolumeChange);
    }

    /// <summary>
    /// Used to stop a sound effect currently playing.
    /// </summary>
    /// <param name="soundName"></param>
    public void StopSoundEffect(EnumSoundName soundName)
    {
        Sound soundEffect = soundEffects.FirstOrDefault(s => s.soundName == soundName);
        if (soundEffect != null)
        {
            soundEffect.source.Stop();
        }
        else
        {
            Debug.Log($"{soundName.ToString()} Sound doesn't exist in AudioManager list of sounds");
        }
    }
    #endregion

    #region private functions 
    /// <summary>
    /// Used to update the volume of the audio source of a list of sounds 
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="sounds"></param>
    void UpdateVolume(float volume, List<Sound> sounds)
    {
        foreach (Sound sound in sounds)
        {
            sound.source.volume = sound.volume * volume;
        }
    }

    /// <summary>
    /// Used to add an audio source component for each sound in the list of sounds
    /// </summary>
    /// <param name="sounds"></param>
    void InitializeAudioSources(List<Sound> sounds)
    {
        // create an audio source for each sound effect in the list of sounds above
        foreach (Sound sound in sounds)
        {
            if (sound.source == null)
            {
                sound.source = gameObject.AddComponent<AudioSource>();
                sound.source.clip = sound.clip;

                sound.source.volume = sound.volume;
                sound.source.pitch = sound.pitch;

                sound.source.loop = sound.loop;
            }
        }
    }

    /// <summary>
    /// Used to manage the singleton. Destroys other instances of audio manager in the scene
    /// to ensure only one audio manager exists.
    /// </summary>
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion
}
