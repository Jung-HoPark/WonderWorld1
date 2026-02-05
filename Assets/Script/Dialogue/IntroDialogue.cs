using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroDialogue : MonoBehaviour
{
    public DialogueManager dialogueManager;
    public List<DialogueLine> introLines;
    void Start()
    {
        Invoke("TriggerIntro", 0.5f);
    }
    void TriggerIntro()
    {
        dialogueManager.StartDialogue(introLines);
    }
    void Update()
    {
        if (dialogueManager.dialoguePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            dialogueManager.DisplayNextSentence();
        }
    }
}
