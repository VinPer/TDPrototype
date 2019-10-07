using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    private Queue<string> sentences;

    public GameObject dialog;
    private TextMeshProUGUI dialogText;
    private void Start()
    {
        sentences = new Queue<string>();
        dialogText = dialog.GetComponent<TextMeshProUGUI>();
        
    }

    public void StartDialogue(Dialogue dialogue)
    {
        //Debug.Log("Starting conversation");

        sentences.Clear();
        foreach (string sentence in dialogue.senteces)
        {
            sentences.Enqueue(sentence);
        }
        Debug.Log("sentence count:  " + sentences.Count);
    }

    public void DisplayNextSentence()
    {
        Debug.Log("xubixubi:  " + sentences.Count);

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        
        string sentence = sentences.Dequeue();
        Debug.Log("tripa seca:  " + sentence);
        //Debug.Log(dialog);

        dialogText.SetText(sentence);
        //Debug.Log(dialogText);
}

    public void EndDialogue()
    {
        //Debug.Log("End of Dialog");
    }
}
