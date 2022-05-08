using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GamePaused = false;
    }
}
