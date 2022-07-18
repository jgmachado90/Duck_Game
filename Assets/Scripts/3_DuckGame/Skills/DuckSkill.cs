using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckSkill : Skill
{
    private bool active;

    [SerializeField] private Sprite ducklingSprite;
    private Sprite mainSprite;
    private SpriteRenderer spriteRenderer;
    public override SkillManager SkillManager { set { skillManager = value; } }
    private SkillManager skillManager;
    public override void FixedUpdateSkill()
    {
       
    }

    public override void UpdateSkill()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeSkill();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            DeactivateSkill();
        }
    }
    public override void InvokeSkill()
    {
        if (active) return;
        active = true;
        skillManager.player.PlayerMovement.InputController.SetInputActive(false);
        spriteRenderer = skillManager.player.MySpriteRenderer;
        mainSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = ducklingSprite;
    }

    public void DeactivateSkill()
    {
        if (!active) return;
        active = false;
        skillManager.player.PlayerMovement.InputController.SetInputActive(true);
        if (spriteRenderer == null) return;
        spriteRenderer.sprite = mainSprite;
    }
}
