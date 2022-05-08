using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroDialoug : MonoBehaviour
{
    [SerializeField]
    private GameObject Player;
    [SerializeField]
    private GameObject Enemy;
    [SerializeField]
    private GameObject HealthBar;
    [SerializeField]
    private GameObject Button;
    [SerializeField]
    private Animator tvScreen;

    [SerializeField]
    private GameObject textBox;
    private TextMeshProUGUI text;
    [SerializeField]
    private string[] textLog;
    private int textIndex = 1;

    void Awake(){
        Player.SetActive(false);
        Enemy.SetActive(false);
        HealthBar.SetActive(false);
        Button.SetActive(false);
        text = textBox.GetComponent<TextMeshProUGUI>();
    }

    private void Update(){
        if(tvScreen.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f){
            textBox.SetActive(true);
            Button.SetActive(true);
        };

    }

    public void textScroll(){
        if (textIndex == textLog.Length){
            StartGame();
            textIndex = 0;
        } else {
            text.text = textLog[textIndex];
            textIndex++;
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