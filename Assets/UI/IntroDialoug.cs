using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialoug : MonoBehaviour
{
    [Header("Objects to turn OFF/ON")]
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject HealthBar;

    [Header("TV Screen UI")]
    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private GameObject textBox;

    [Header("TV Animator")]
    [SerializeField]
    private Animator tvScreen;
    
    [Header("Text Box")]
    private TextMeshProUGUI text;
    [SerializeField]
    [TextArea]
    private string[] textLog;
    private int textIndex = 0;

    void Awake(){
        Player.SetActive(false);
        Enemy.SetActive(false);
        HealthBar.SetActive(false);
        Button.SetActive(false);
        text = textBox.GetComponent<TextMeshProUGUI>();
    }

    private void Update(){
        if(tvScreen.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f){ // check that the tv animation is done
            textBox.SetActive(true);
            Button.SetActive(true);
            if(textIndex == 0)
            {
                TypeText();
            }
        };

    }

    public void textScroll(){
        // if it's the last test dialog, and the type writer has finished typing. start game
        if (textIndex == textLog.Length && TypeWriter.instance?.isCurrentlyTyping == false){
            StartGame();
            textIndex = 1;
        } else {
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

            bool startedTypeingLine = TypeWriter.instance.TypeWriteLine(lineToType, text);

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
        yield return new WaitForSeconds(1f);
        tvScreen.gameObject.SetActive(false);
        textBox.SetActive(false);
        Player.SetActive(true);
        Enemy.SetActive(true);
        HealthBar.SetActive(true);
        Button.SetActive(false);
        
    }
}
