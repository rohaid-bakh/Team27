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

    [Header("Player")]
    [SerializeField]
    private PlayerCharacterController Player;

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
    private int tvTextIndex = 1;

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

    private void Awake()
    {
        tvScreenImage.enabled = false;
        tvScreenAnimator.enabled = false;
    }

    public void StartOutroDialog(){
        // first disable player controls (we still want to see the player)
        Player.CanControlPlayer = false;

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
    }

    public void textScroll(){
        // if it's the second last outro dialogue, show player dialogue
        if (tvTextIndex == tvTextLog.Length - 2) {
            
            ShowPlayerDialogue();
            tvTextIndex++;
        }
        // if it's the last outro dialogue, hide player dialog
        else if (tvTextIndex == tvTextLog.Length - 1)
        {
            HidePlayerDialogue();
            tvText.text = tvTextLog[tvTextIndex];
            tvTextIndex++;
        }
        // no more dialog, transition to the end scene
        else if (tvTextIndex >= tvTextLog.Length)
        {
            tvButton.SetActive(false);
            StartCoroutine(EndGame()); // transition to outro scene
        }
        else {
            tvText.text = tvTextLog[tvTextIndex];
            tvTextIndex++;
        }
    }

    public void ShowPlayerDialogue()
    {
        // hide tv text
        tvText.text = "";

        dialogTextBox.SetActive(true);
        playerSprite.sprite = playerDialoug.DialougeSprites[0]; // set up victory quote
        dialogText.text = "Wait… what about me? Do I get to be an honorary Krokor? For completing Ogenjjak?";
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
