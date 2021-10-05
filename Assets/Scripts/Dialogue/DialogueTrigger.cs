using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
public class DialogueTrigger : Entity
{
    public Dialogue dialogue;

    public TriggerArea triggerArea;

    public Feedback endConversation;

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
            OwnerLevel.dialogueManager.currentDialogue = null;
        }
    }


    private void OnEnterDialogueArea(TriggerArea obj)
    {
        OwnerLevel.dialogueManager.currentDialogue = this;
    }
    private void ExitDialogueArea(TriggerArea obj)
    {
        OwnerLevel.dialogueManager.currentDialogue = null;
    }

    public void EndConversation()
    {
        endConversation?.Invoke(this);
    }

}
