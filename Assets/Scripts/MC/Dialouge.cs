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

    System.Random random;

    private void Awake(){
        //Adds a listener to the main slider and invokes a method when the value changes.
        enemy.onValueChanged.AddListener(delegate {Victory(); });
        player.onValueChanged.AddListener(delegate {Loss(); });

        random = new System.Random();
    }

    public void Victory()
    {
     if(enemy.value == 0){
            
         // start crowd cheer
         FindObjectOfType<CrowdCheer>()?.CrowdCheering();

         // first disable player controls (we still want to see the player)
         Player.CanControlPlayer = false;
                    
         // victory quote (picks a random victory quote)
         List<int> victoryQuoteIndexes = new List<int> { 0, 1 };
         int randomVictoryQuoteIndex = victoryQuoteIndexes[random.Next(victoryQuoteIndexes.Count)];
         string victoryQuote = playerDialoug.Dialouge[randomVictoryQuoteIndex]; // TODO: add randomizer to choose different quotes
                     
         // victory sprite
         Sprite victorySprite = playerDialoug.DialougeSprites[0];
         
            // get current scene
         Scene scene = SceneManager.GetActiveScene();

         if(scene.name.Trim() == "BorealFight") // if it's the final boss, load outro dialog (TODO: replace with the real final scene name)
         {
            StartCoroutine(DialogAndStartVictorySceneDialog(victoryQuote));
         } // otherwise load next scene
         else 
         {
            int nextScene = scene.buildIndex + 1; // use this instead when actual level build index is finalized
            StartCoroutine(DialogAndSceneTransition(victoryQuote, victorySprite, nextScene)); 
         }
         
     }  
    }

    public void Loss(){ 
        if(player.value == 0){

            // start crowd cheer
            FindObjectOfType<CrowdCheer>()?.CrowdCheering();

            // first disable player controls (we still want to see the player)
            Player.CanControlPlayer = false;

            // loss quote (picks a random loss quote)
            List<int> lossQuoteIndexes = new List<int> { 2, 3 };
            int randomLossQuoteIndex = lossQuoteIndexes[random.Next(lossQuoteIndexes.Count)];
            string lossQuote = playerDialoug.Dialouge[randomLossQuoteIndex];

            // loss sprite
            Sprite lossSprite = playerDialoug.DialougeSprites[1];

            int loseScreenIndex = 3; // TODO: once build index is finalized, double check if this is okay
            StartCoroutine(DialogAndSceneTransition(lossQuote, lossSprite, loseScreenIndex));
        }
    }

    private IEnumerator DialogAndStartVictorySceneDialog(string victoryQuote)
    {
        // no text to start
        text.text = "";

        // wait half a second before dialog text appears
        yield return new WaitForSeconds(0.5f);

        // show dialog box
        textBox.SetActive(true);

        // wait half a quarter second before dialog text appears
        yield return new WaitForSeconds(0.25f);

        playerSprite.sprite = playerDialoug.DialougeSprites[0];
        TypeWriter.instance?.TypeWriteLine(victoryQuote, text);

        // wait for text to finish typing
        yield return new WaitUntil(() => TypeWriter.instance.isCurrentlyTyping == false);

        // wait for 2 seconds
        yield return new WaitForSeconds(2f);

        textBox.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<OutroDialoug>().StartOutroDialog();
    }

    private IEnumerator DialogAndSceneTransition(string victoryOrLossQuote, Sprite victoryOrLossSprite, int scene){
        // no text to start
        text.text = "";

        // wait half a second before dialog text appears
        yield return new WaitForSeconds(0.5f);

        // show dialog box
        textBox.SetActive(true);

        // wait half a quarter second before dialog text appears
        yield return new WaitForSeconds(0.25f);

        playerSprite.sprite = victoryOrLossSprite;
        TypeWriter.instance?.TypeWriteLine(victoryOrLossQuote, text);

        // wait for text to finish typing
        yield return new WaitUntil(() => TypeWriter.instance.isCurrentlyTyping == false);

        // wait for 2 seconds
        yield return new WaitForSeconds(2f);

        //transition
        sceneTransition.SetTrigger("startTransit");
        
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(scene);
        
    }

}
