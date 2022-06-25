using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameFramework;

namespace DialogueSystem
{
    public class DialogueManager : Entity, ISubsystem<LevelManager>
    {
        public LevelManager OwnerManager{ get; set; }
        [SerializeField] private InputController inputController;

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
            if (inputController == null)
                inputController = this.GetLevelManagerSubsystem<InputController>();
        }

        private void Update()
        {
            if(currentDialogue != null && !talking)
            {
                StartDialogue();
            }
            else if (talking && inputController.GetDialogueInput())
            {
                DisplayNextSentence();
            }
        }

        public void StartDialogue()
        {
            talking = true;
            dialogueUI.gameObject.SetActive(true);
            Dialogue dialogue = currentDialogue.dialogue;

            nameText.text = dialogue.name;

            sentences.Clear();

            foreach (string sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        private void DisplayNextSentence()
        {
            if (sentences.Count == 0)
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
            foreach (char letter in sentence.ToCharArray())
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
}