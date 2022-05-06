using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("this is where the fun begins");
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex + 1);
    }
}
