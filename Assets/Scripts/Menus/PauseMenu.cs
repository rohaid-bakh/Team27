using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused = false;

    public GameObject PauseMenuUI;

    void Update()
    {
        if ( Keyboard.current.eKey.wasPressedThisFrame) //  Input.GetKeyDown(KeyCode.E))
        {
            GamePaused = !GamePaused;

            if (GamePaused)
            {
                AudioManager.instance?.PauseAllSoundEffects();
                PauseMenuUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else {
                UnPause();
            }
        }
    }

    // this is seperate so button can call it
    public void UnPause () 
    {
        AudioManager.instance?.UnPauseAllSoundEffects();
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }

    public void MainMenu()
    {
        StartCoroutine(LoadMainMenu());
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(0f); // todo: adjust

        Time.timeScale = 1f;
        
        GamePaused = false;

        SceneManager.LoadScene("MainMenu");
    }
}
