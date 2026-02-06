using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public PlayerController player;

    public float typingSpeed = 0.05f;
    private Queue<DialogueLine> sentences;
    private bool isTyping = false;
    private string currentSentence;
    public Action OnDialogueComplete;

    private void Awake()
    {
        sentences = new Queue<DialogueLine>();
        dialoguePanel.SetActive(false);
    }
    public void StartDialogue(List<DialogueLine> lines)
    {
        if (player != null) player.canMove = false;

        dialoguePanel.SetActive (true);
        sentences.Clear();

        foreach(DialogueLine line in lines)
        {
            sentences.Enqueue(line);
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.text = currentSentence;
            isTyping = false;
            return;
        }

        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = sentences.Dequeue();
        currentSentence = line.sentence;

        StartCoroutine(TypeSentence(line.sentence));
    }
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
    void EndDialogue()
    {
        if (player != null) player.canMove = true;
        dialoguePanel.SetActive(false);
        OnDialogueComplete?.Invoke();
        OnDialogueComplete = null;
        Debug.Log("대화 종료");

        IntroSceneManager introManager = FindObjectOfType<IntroSceneManager>();
        if (introManager != null)
        {
            introManager.OnIntroComplete();
        }
    }
}
