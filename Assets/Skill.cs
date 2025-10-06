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

    [Header("技能列表")]
    public List<SkillData> skills = new List<SkillData>();

    void Update()
    {
        // 检测技能按键 - 修复：添加了isUnlocked检查
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
        // 先锁定所有技能
        foreach (var skill in skills)
        {
            skill.isUnlocked = false;
        }
        
        // 然后解锁指定的技能
        foreach (var skill in skills)
        {
            if (skill.skillName == skillName)
            {
                skill.isUnlocked = true;
                Debug.Log($"已解锁技能: {skillName}");
            }
        }
    }
    
    // 新增：锁定所有技能
    public void LockAllSkills()
    {
        foreach (var skill in skills)
        {
            skill.isUnlocked = false;
        }
        Debug.Log("所有技能已锁定");
    }
}