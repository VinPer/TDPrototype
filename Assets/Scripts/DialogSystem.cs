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
    private IDictionary<int, string> levelBtn;


    private void Start()
    {
        sentences = new Queue<string>();
        dialogText = dialog.GetComponent<TextMeshProUGUI>();
        levelBtn = new Dictionary<int, string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("Starting conversation");

        //Como achei que seria:
        for(int i = 0; i < dialogue.senteces.Length; i++)
        {
            levelBtn.Add(i + 1, dialogue.senteces[i]);
        }

        //Como tive que fazer
        //levelBtn.Add(1, dialogue.senteces[0]);
        //levelBtn.Add(2, dialogue.senteces[1]);
        //levelBtn.Add(3, dialogue.senteces[2]);
        //levelBtn.Add(4, dialogue.senteces[3]);
        //levelBtn.Add(5, dialogue.senteces[4]);
        //levelBtn.Add(6, dialogue.senteces[5]);
    }

    public void DisplayNextSentence(int numeroBtn)
    {
        //Debug.Log("xubixubi:  " + levelBtn.Count);

        string sentence = levelBtn[numeroBtn];
        //Debug.Log("tripa seca:  " + sentence);

        dialogText.SetText(sentence);
    }

}
