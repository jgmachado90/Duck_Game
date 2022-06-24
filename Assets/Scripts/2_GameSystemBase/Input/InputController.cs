using UnityEngine;
using GameFramework;

public class InputController : Entity, ISubsystem<LevelManager>
{
    public LevelManager OwnerManager { get; set; }

    private bool inputActive = true;

    public void SetInputActive(bool value)
    {
        inputActive = value;
    }

    public Vector2 GetMovementInput()
    {
        if (inputActive)
            return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        return new Vector2(0,0);
    }

    public bool GetDialogueInput()
    {
        return Input.GetKeyDown(KeyCode.Z);
    }


}
