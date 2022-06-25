using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using TMPro;

namespace DialogueSystem
{

    public class NotificationManager : Entity, ISubsystem<LevelManager>
    {
        public LevelManager OwnerManager { get; set; }
        [SerializeField] private InputController inputController;

        [SerializeField] Transform notificationUI;
        [SerializeField] private TextMeshProUGUI notificationText;
        [SerializeField] private float timePerLetter;

        private bool notificating;
        public NotificationComponent currentNotification;

        private Queue<string> sentences = new Queue<string>();

        private void Start()
        {
            currentNotification = null;
            notificating = false;
            if (inputController == null)
                inputController = this.GetLevelManagerSubsystem<InputController>();
        }

        private void Update()
        {
            if (currentNotification != null && !notificating)
            {
                Notificate();
            }
            else if (notificating && inputController.GetDialogueInput())
            {
                EndNotification();
            }
        }

        public void Notificate()
        {
            notificating = true;
            notificationUI.gameObject.SetActive(true);
            Dialogue dialogue = currentNotification.dialogue;

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
                EndNotification();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        IEnumerator TypeSentence(string sentence)
        {
            notificationText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                notificationText.text += letter;
                yield return new WaitForSeconds(timePerLetter);
            }
        }

        public void ForceEndNotification()
        {
            Debug.Log("ForceEndDialog");
            notificating = false;
            if(currentNotification != null)
                currentNotification.ForceEndNotification();
            notificationUI.gameObject.SetActive(false);
        }


        public void EndNotification()
        {
            Debug.Log("EndOfConversation");
            notificating = false;
            currentNotification.EndNotification();
            notificationUI.gameObject.SetActive(false);
        }
    }
}
