using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMainMusicTheme : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        AudioManager.instance?.PlayMusicTrack(EnumSoundName.MainTheme);
    }
}