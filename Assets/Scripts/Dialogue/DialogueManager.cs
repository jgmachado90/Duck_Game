using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] Transform dialogueUI;
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float timePerLetter;

    private bool talking;
    public DialogueTrigger currentDialogue; 

    private Queue<string> sentences = new Queue<string>();

    private void Start()
    {
        currentDialogue = null;
        talking = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentDialogue != null && talking == false)
                StartDialogue();
            else if (talking == true)
            {
                DisplayNextSentence();
            }
        }
        
    }

    public void StartDialogue()
    {
        talking = true;
        dialogueUI.gameObject.SetActive(true);
        Dialogue dialogue = currentDialogue.dialogue;

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timePerLetter);
        }
    }

    void EndDialogue()
    {
        Debug.Log("EndOfConversation");
        talking = false;
        currentDialogue.EndConversation();
        dialogueUI.gameObject.SetActive(false);
    }
}
