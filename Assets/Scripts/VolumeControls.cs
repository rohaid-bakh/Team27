using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour
{
    AudioManager audioManager;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider soundEffectVolumeSlider;

    bool isStarting = true;

    void Start()
    {
        isStarting = true;

        audioManager = FindObjectOfType<AudioManager>();

        // update volume sliders
        musicVolumeSlider.value = AudioManager.instance.MusicVolume;
        soundEffectVolumeSlider.value = AudioManager.instance.SoundEffectVolume;

        isStarting = false;
    }

    public void OnUpdateMusicVolume(float volume)
    {
        // only want this function to work after the Start() has completed
        if(!isStarting)
            AudioManager.instance?.UpdateMusicVolume(volume);
    }

    public void OnUpdateSoundEffectVolume(float volume)
    {
        // only want this function to work after the Start() has completed
        if (!isStarting)
            AudioManager.instance?.UpdateSoundEffectVolume(volume, playSoundEffect:true);
    }
}