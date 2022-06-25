using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

namespace DialogueSystem {
    public class NotificationComponent : Entity
    {
        private NotificationManager notificationManager;
        public Feedback endConversation;

        public Dialogue dialogue;

        public bool onlyOnce;

        public void Start()
        {
            notificationManager = this.GetLevelManagerSubsystem<NotificationManager>();
        }

        public void ActivateNotification()
        {
            notificationManager.currentNotification = this;
        }
        public void DeactivateNotification()
        {
            notificationManager.ForceEndNotification();
        }
        public void ForceEndNotification()
        {
            notificationManager.currentNotification = null;
        }

        public void EndNotification()
        {
            notificationManager.currentNotification = null;
            endConversation?.Invoke(this);
            if (onlyOnce)
                gameObject.SetActive(false);
        }
    } 
}
