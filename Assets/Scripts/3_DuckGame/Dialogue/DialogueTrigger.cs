using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using CollisionSystem;
using MoreMountains.Feedbacks;


namespace DialogueSystem
{
    public class DialogueTrigger : Entity
    {
        private DialogueManager dialogueManager;

        private InputController inputController;
    
        public bool onlyOnce;
        public bool lockPlayer;

        public Dialogue dialogue;

        public TriggerArea triggerArea;

        public MMFeedbacks endConversation;

        public DialogueManager CurrentManager { get; set; }

        private void OnEnable()
        {
            if (triggerArea)
            {
                triggerArea.OnEnterArea += OnEnterDialogueArea;
                triggerArea.OnExitArea += ExitDialogueArea;
            }
        }

        private void OnDisable()
        {
            if (triggerArea)
            {
                triggerArea.OnEnterArea -= OnEnterDialogueArea;
                triggerArea.OnExitArea -= ExitDialogueArea;
            }
            if (CurrentManager)
            {
                this.GetLevelManagerSubsystem<DialogueManager>(this) .currentDialogue = null;
            }
        }

        private void Start()
        {
            dialogueManager = this.GetLevelManagerSubsystem<DialogueManager>();
            inputController = this.GetLevelManagerSubsystem<InputController>();
        }

        private void OnEnterDialogueArea(GameObject obj)
        {
            if (lockPlayer)
                inputController.SetInputActive(false);
            dialogueManager.currentDialogue = this;
        }
        private void ExitDialogueArea(GameObject obj)
        {
            dialogueManager.currentDialogue = null;
        }

        public void EndConversation()
        {
            dialogueManager.currentDialogue = null;
            endConversation.PlayFeedbacks();
            if (lockPlayer)
                inputController.SetInputActive(true);
            if (onlyOnce)
                gameObject.SetActive(false);
        }

    }
}