using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class SkillManager : Entity
{
    public Player player;
    private InputController inputController;
    [SerializeField] private List<Skill> startingSkills;

    [SerializeField] private List<Skill> currentSkills;

    private void Start()
    {
        if (inputController == null)
            inputController = this.GetLevelManagerSubsystem<InputController>();
        player = GetComponentInParent<Player>();

        foreach(Skill skill in startingSkills)
        {
            skill.SkillManager = this;
            currentSkills.Add(skill);
        }
    }

    private void Update()
    {
        foreach(Skill skill in currentSkills)
        {
            skill.UpdateSkill();
        }
    }


}
