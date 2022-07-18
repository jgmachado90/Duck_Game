using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    public abstract SkillManager SkillManager { set; }
    public abstract void UpdateSkill();
    public abstract void FixedUpdateSkill();

    public abstract void InvokeSkill();
}
