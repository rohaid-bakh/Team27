using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevels : MonoBehaviour
{
   public void LoadStart(){
       SceneManager.LoadScene("MagogFight");
   }
   public void LoadMain(){
       SceneManager.LoadScene("MainMenu");
   }
}
