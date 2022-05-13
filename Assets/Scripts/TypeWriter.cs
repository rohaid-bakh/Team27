using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TypeWriter : MonoBehaviour
{
    [SerializeField]
    private float typingSpeed = 0.04f;

    Coroutine displayLineCoroutine = null;
    private Image continueButtonImage = null;

    string currentTypingLine = "";
    public bool isCurrentlyTyping { get { return currentTypingLine == "" ? false : true; } }

    public static TypeWriter instance;

    // Use this for initialization
    private void Awake()
    {
        // delete other instances of the type writer
        ManageSingleton();
    }

    public bool TypeWriteLine(string newLineToType, TextMeshProUGUI text, Image dialogContinueButtonImage = null)
    {
        bool startedTypingNewLine = false;

        // stop coroutine
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);

        // if currently typing, finish the line currently being typed
        if (isCurrentlyTyping)
        {
            // first, finish typing the current line, and show button
            text.text = currentTypingLine;

            // show continue button after finished
            if(continueButtonImage != null)
                continueButtonImage.enabled = true;

            currentTypingLine = "";
        }
        else
        {
            // hide continue button 
            if (continueButtonImage != null)
                continueButtonImage.enabled = false;

            continueButtonImage = dialogContinueButtonImage;
            displayLineCoroutine = StartCoroutine(TypeWriteLineCoroutine(newLineToType, text));
            startedTypingNewLine = true;
        }

        return startedTypingNewLine;
    }

    public void StopTyping()
    {
        if (displayLineCoroutine != null)
            StopCoroutine(displayLineCoroutine);
        currentTypingLine = "";

        // show continue button 
        if (continueButtonImage != null)
            continueButtonImage.enabled = true;
    }

    private IEnumerator TypeWriteLineCoroutine(string line, TextMeshProUGUI text)
    {
        currentTypingLine = line;

        // emptry the dialogue text
        text.text = "";

        // idsplay each letter one at a time
        foreach (char letter in line.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        currentTypingLine = "";

        // show continue button 
        if (continueButtonImage != null)
            continueButtonImage.enabled = true;
    }

    /// <summary>
    /// Used to manage the singleton. Destroys other instances in scene
    /// </summary>
    void ManageSingleton()
    {
        if (instance != null)
        {
            // need to disable this so other objects don't try to access
            gameObject.SetActive(false);

            // now destroy
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}
