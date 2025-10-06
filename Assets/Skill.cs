using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSystem : MonoBehaviour
{
    [System.Serializable]
    public class SkillData
    {
        public string skillName;
        public KeyCode triggerKey;
        public float cooldown = 5f;
        public bool isUnlocked = false;
        public bool isOnCooldown = false;
        public System.Action onExecute;
    }

    public List<SkillData> skills = new List<SkillData>();

    void Update()
    {
        foreach (var skill in skills)
        {
            if (skill.isUnlocked && !skill.isOnCooldown && Input.GetKeyDown(skill.triggerKey))
            {
                ExecuteSkill(skill);
            }
        }
    }

    void ExecuteSkill(SkillData skill)
    {
        skill.onExecute?.Invoke();
        StartCoroutine(SkillCooldown(skill));
    }

    IEnumerator SkillCooldown(SkillData skill)
    {
        skill.isOnCooldown = true;
        yield return new WaitForSeconds(skill.cooldown);
        skill.isOnCooldown = false;
    }

    public void UnlockSkill(string skillName)
    {
        foreach (var skill in skills)
        {
            skill.isUnlocked = (skill.skillName == skillName);
        }
    }
}