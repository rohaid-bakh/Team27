using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntroDialoug : MonoBehaviour
{
    [Header("Objects to turn OFF/ON")]
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject HealthBar;

    [Header("Tv Text Dialogue")]
    [SerializeField] Image continueButtonImage;
    [SerializeField]
    [TextArea]
    private string[] textLog;
    [SerializeField] bool showTutorialImage = false;
    [SerializeField] GameObject tutorialImages;

    private int textIndex = 0;

    private TextMeshProUGUI text;
    private Animator tvScreen;
    private Button button;

    void Awake(){
        Player.SetActive(false);
        Enemy.SetActive(false);
        HealthBar.SetActive(false);
        text = GetComponentInChildren<TextMeshProUGUI>();
        tvScreen = GetComponentInChildren<Animator>();
        button = GetComponentInChildren<Button>();

        // disable button
        button.enabled = false;
        continueButtonImage.enabled = false;

        // hide text
        text.text = "";
    }

    private void Update(){
        if(tvScreen.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f){ // check that the tv animation is done
            if(textIndex == 0)
            {
                button.enabled = true;
                TypeText();
            }
        };
    }

    public void textScroll(){
        // if it's the last test dialog, and the type writer has finished typing. start game
        if (textIndex == textLog.Length && TypeWriter.instance?.isCurrentlyTyping == false){
            StartGame();
            textIndex = 1;
        }
        // shows tutorial for the last text on the first scene
        else if(textIndex == textLog.Length - 1 && showTutorialImage && tutorialImages != null && TypeWriter.instance?.isCurrentlyTyping == false)
        {
            tutorialImages.SetActive(true);
            TypeText();
        }
        else {
            TypeText();
        }
    }

    // types the text using the type writer class
    private void TypeText()
    {
        // check if type writer exists in scene
        if (TypeWriter.instance != null)
        {
            // get line to type from the text log (if textIndex is less than the length)
            string lineToType = textIndex < textLog.Length ? textLog[textIndex] : "";

            bool startedTypeingLine = TypeWriter.instance.TypeWriteLine(lineToType, text, continueButtonImage);

            // if the type writer started typing the new line, increase textIndex (otherwise, type writer was finishing a line currently being typed)
            if (startedTypeingLine)
                textIndex++;
        }
        else
        {
            Debug.Log("TypeWriterMonoBehaviour doesn't exist in the current scene, so texts are not being written.");
        }
    }

    private void StartGame(){
        tvScreen.SetTrigger("ScrollOut");
        StartCoroutine(setUpUI());
        
    }

    private IEnumerator setUpUI(){

        // hide text & disable button
        text.text = "";
        button.enabled = false;
        continueButtonImage.enabled = false;

        // hide tutorial if applicable
        if(tutorialImages != null)
            tutorialImages.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        tvScreen.gameObject.SetActive(false);
        Player.SetActive(true);
        Enemy.SetActive(true);
        HealthBar.SetActive(true);
        gameObject.SetActive(false);
    }
}
