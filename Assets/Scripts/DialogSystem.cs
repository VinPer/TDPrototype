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

        for(int i = 0; i < dialogue.senteces.Length; i++)
        {
            levelBtn.Add(i + 1, dialogue.senteces[i]);
        }

    }

    public void DisplayNextSentence(int numeroBtn)
    {

        string sentence = levelBtn[numeroBtn];

        dialogText.SetText(sentence);
    }

}
