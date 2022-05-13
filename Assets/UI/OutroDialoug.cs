using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutroDialoug : MonoBehaviour
{
    [Header("Objects to turn ON/OFF")]
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject HealthBar;

    [Header("TV Screen UI")]
    [SerializeField]
    private GameObject tvButton;
    [SerializeField]
    private GameObject tvTextBox;

    [Header("TV Animator")]
    [SerializeField]
    private Image tvScreenImage;
    [SerializeField]
    private Animator tvScreenAnimator;
    
    [Header("Tv Text Box")]
    private TextMeshProUGUI tvText;
    [SerializeField]
    [TextArea]
    private string[] tvTextLog;
    private int tvTextIndex = 0;

    [Header("Player Dialouge Box")]
    [SerializeField]
    private GameObject dialogTextBox;
    [SerializeField]
    private TextMeshProUGUI dialogText;
    [SerializeField]
    private Stats playerDialoug;

    [Header("Player Dialouge Sprite")]
    [SerializeField]
    private Image playerSprite;
    string playerDialogLine = "Wait. . . what about me? Do I get to be an honorary Krokor? For completing Ogenjjak?";

    private void Awake()
    {
        tvScreenImage.enabled = false;
        tvScreenAnimator.enabled = false;
    }

    public void StartOutroDialog(){
        // hide enemy & health bar
        Enemy.SetActive(false);
        HealthBar.SetActive(false);

        // enable tv animator & wait for animation to finish
        tvText = tvTextBox.GetComponent<TextMeshProUGUI>();
        StartCoroutine(WaitForTvAnimationToFinish());
    }

    IEnumerator WaitForTvAnimationToFinish()
    {
        tvScreenImage.enabled = true;
        tvScreenAnimator.enabled = true;

        yield return new WaitUntil(() => tvScreenAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);

        tvTextBox.SetActive(true);
        tvButton.SetActive(true);

        TypeTvText();
    }

    public void textScroll(){
        // if it's the second last outro dialogue, and if the dialog is finished typing, show player dialogue
        if (tvTextIndex == tvTextLog.Length - 2 && TypeWriter.instance?.isCurrentlyTyping == false) {
            
            ShowPlayerDialogue();
        }
        // if it's the last outro dialogue, if the dialog is finished typing, hide player dialog
        else if (tvTextIndex == tvTextLog.Length - 1 && TypeWriter.instance?.isCurrentlyTyping == false)
        {
            HidePlayerDialogue();
            TypeTvText();
        }
        // if it's the last outro dialogue, if the dialog is currently typing, finish the player dialog line
        else if (tvTextIndex == tvTextLog.Length - 1 && TypeWriter.instance?.isCurrentlyTyping == true)
        {
            // stop typewriter
            TypeWriter.instance?.StopTyping();

            // finish typing the player dialog
            dialogText.text = playerDialogLine;
        }
        // no more dialog, transition to the end scene
        else if (tvTextIndex >= tvTextLog.Length && TypeWriter.instance?.isCurrentlyTyping == false)
        {
            tvButton.SetActive(false);
            StartCoroutine(EndGame()); // transition to outro scene
        }
        else
        {
            TypeTvText();
        }
    }

    // types the text using the type writer class
    private void TypeTvText()
    {
        // check if type writer exists in scene
        if (TypeWriter.instance != null)
        {
            // get line to type from the text log (if textIndex is less than the length)
            string lineToType = tvTextIndex < tvTextLog.Length ? tvTextLog[tvTextIndex] : "";

            bool startedTypeingLine = TypeWriter.instance.TypeWriteLine(lineToType, tvText);

            // if the type writer started typing the new line, increase textIndex (otherwise, type writer was finishing a line currently being typed)
            if (startedTypeingLine)
                tvTextIndex++;
        }
        else
        {
            Debug.Log("TypeWriterMonoBehaviour doesn't exist in the current scene, so texts are not being written.");
        }
    }

    public void ShowPlayerDialogue()
    {
        // hide tv text
        tvText.text = "";

        // show dialog box
        dialogTextBox.SetActive(true);
        playerSprite.sprite = playerDialoug.DialougeSprites[0]; // set up sprite

        if (TypeWriter.instance != null)
        {
            bool startedTypeingLine = TypeWriter.instance.TypeWriteLine(playerDialogLine, dialogText);

            // if the type writer started typing the new line, increase tvIndex
            if (startedTypeingLine)
                tvTextIndex++;
        }
        else
        {
            Debug.Log("TypeWriterMonoBehaviour doesn't exist in the current scene, so texts are not being written.");
        }
    }

    public void HidePlayerDialogue()
    {
        dialogTextBox.SetActive(false);
    }

    private IEnumerator EndGame(){
        tvScreenAnimator.SetTrigger("ScrollOut");

        // load main menu
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}
