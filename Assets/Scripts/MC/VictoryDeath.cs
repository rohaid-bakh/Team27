using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class VictoryDeath : MonoBehaviour
{
    [SerializeField]
    private Slider enemy;
    [SerializeField]
    private Slider player;
    [SerializeField]
    private Animator sceneTransition;

    private GameObject textBox;

    private void Awake(){
        //Adds a listener to the main slider and invokes a method when the value changes.
        enemy.onValueChanged.AddListener(delegate {Victory(); });
        player.onValueChanged.AddListener(delegate {Loss(); });
    }

    // Invoked when the value of the slider changes.
    public void Victory()
    {
     if(enemy.value == 0){
         StartCoroutine(NewLevel());
     }  
    }

    public IEnumerator NewLevel(){
        yield return new WaitForSeconds(1f);
    }
    public void Loss(){
        if(player.value == 0){
            StartCoroutine(SceneTransition());
            SceneManager.LoadScene("LoseScreen");
        }
    }

    private IEnumerator SceneTransition(){
        sceneTransition.SetTrigger("startTransit");
        yield return new WaitForSeconds(1f);
    }

}
