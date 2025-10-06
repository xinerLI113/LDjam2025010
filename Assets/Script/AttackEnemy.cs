using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnemy : MonoBehaviour
{
    public float lifeLevel = 100;
    public float skillValue = 0; // 默认改为0
    public float skillOfQValue;
    public float swordHurts;
    
    // 保存原始伤害值（用于在平台上时恢复）
    private float originalSkillValue = 1;
    
    void Start()
    {
        // 初始化时所有伤害都为0
        UpdateSkillValues();
    }

    void Update()
    {
        if (lifeLevel <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Skills"))
        {
            lifeLevel -= skillOfQValue;
        }
    }
    
    // 更新所有相关伤害值
    public void UpdateSkillValues()
    {
        skillOfQValue = skillValue * 10;
        swordHurts = skillValue * 10;
    }
    
    // 设置伤害状态的方法
    public void SetDamageEnabled(bool enabled)
    {
        if (enabled)
        {
            skillValue = originalSkillValue; // 站在平台上时恢复伤害
        }
        else
        {
            skillValue = 0; // 不在平台上时伤害为0
        }
        UpdateSkillValues();
    }
}