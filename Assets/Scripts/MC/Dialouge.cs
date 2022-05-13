using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Dialouge : MonoBehaviour
{
    [Header("Player")]
    [SerializeField]
    private PlayerCharacterController Player; // to turn off player controls on victory/loss

    [Header("Sliders")]
    [SerializeField]
    private Slider enemy;
    [SerializeField]
    private Slider player;

    [Header("Scene Transition")]
    [SerializeField]
    private Animator sceneTransition;

    [Header("Dialouge Box")]
    [SerializeField]
    private GameObject textBox;
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Stats playerDialoug;

    [Header("Dialouge Sprite")]
    [SerializeField]
    private Image playerSprite;

    private void Awake(){
        //Adds a listener to the main slider and invokes a method when the value changes.
        enemy.onValueChanged.AddListener(delegate {Victory(); });
        player.onValueChanged.AddListener(delegate {Loss(); });
    }

    public void Victory() // TODO: add randomizer to choose different quotes
    {
     if(enemy.value == 0){
         // first disable player controls (we still want to see the player)
         Player.CanControlPlayer = false;

         textBox.SetActive(true);

         playerSprite.sprite = playerDialoug.DialougeSprites[0]; // set up victory quote
         TypeWriter.instance?.TypeWriteLine(playerDialoug.Dialouge[0], text); // set up victory dialouge
        
         Scene scene = SceneManager.GetActiveScene();

         if(scene.name.Trim() == "MagogFight") // if it's the final boss, load outro dialog (TODO: replace with the real final scene name)
         {
            StartCoroutine(StartVictorySceneDialog());
         } // otherwise load next scene
         else 
         {
            int nextScene = scene.buildIndex + 1; // use this instead when actual level build index is finalized
            StartCoroutine(SceneTransition(2)); // change the passed in when the actual level build index is finalized
         }
         
     }  
    }

    public void Loss(){ // TODO: add randomizer to choose different quotes
        if(player.value == 0){
            // first disable player controls (we still want to see the player)
            Player.CanControlPlayer = false;

            textBox.SetActive(true);
            playerSprite.sprite = playerDialoug.DialougeSprites[1];
            TypeWriter.instance?.TypeWriteLine(playerDialoug.Dialouge[2], text);
            StartCoroutine(SceneTransition(2));
        }
    }

    private IEnumerator StartVictorySceneDialog()
    {
        // wait for text to finish typing
        yield return new WaitUntil(() => TypeWriter.instance.isCurrentlyTyping == false);

        // wait for one second
        yield return new WaitForSeconds(1f);

        textBox.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<OutroDialoug>().StartOutroDialog();
    }

    private IEnumerator SceneTransition(int scene){

        // wait for text to finish typing
        yield return new WaitUntil(() => TypeWriter.instance.isCurrentlyTyping == false);

        // wait for one second
        yield return new WaitForSeconds(1f);

        //transition
        sceneTransition.SetTrigger("startTransit");
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
        
    }

}
