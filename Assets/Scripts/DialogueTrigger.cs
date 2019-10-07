using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private void Start()
    {
        FindObjectOfType<DialogSystem>().StartDialogue(dialogue);
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogSystem>().DisplayNextSentence();

    }
}
