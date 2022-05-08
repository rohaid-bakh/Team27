using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingLevels : MonoBehaviour
{
   public void LoadStart(){
       SceneManager.LoadScene(1);
   }
   public void LoadMain(){
       SceneManager.LoadScene(0);
   }
}