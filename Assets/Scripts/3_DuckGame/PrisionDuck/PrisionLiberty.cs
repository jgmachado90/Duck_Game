using UnityEngine;
using CollisionSystem;
using GameFramework;
using DialogueSystem;

public class PrisionLiberty : Entity
{
    public TriggerArea triggerArea;
    private InputController inputController;
    [SerializeField] private NotificationComponent notificationComponent;

    private void OnEnable()
    {
        if (triggerArea != null)
        {
            triggerArea.OnEnterArea += OnEnterLibertyArea;
            triggerArea.OnExitArea += OnExitLibertyArea;
        }
    }

    private void OnDisable()
    {
        if (triggerArea != null)
        {
            triggerArea.OnEnterArea -= OnEnterLibertyArea;
            triggerArea.OnExitArea -= OnExitLibertyArea;
        }
    }

    private void Start()
    {
        inputController = this.GetLevelManagerSubsystem<InputController>();
    }

    private void OnEnterLibertyArea(GameObject obj)
    {
        notificationComponent.ActivateNotification();
    }
    private void OnExitLibertyArea(GameObject obj)
    {
        notificationComponent.DeactivateNotification();
    }
}
