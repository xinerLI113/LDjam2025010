using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsCardManager : MonoBehaviour
{
    public Button lockHP;
    public Button spawn;
    public Button farAttack;
    public Button renew;
    public Button platformCard;
    
    [Header("引用")]
    public LifeSystem lifeSystem;
    public Platform platform;
    public GameObject playerPrefab;
    public Transform spawnPoint;
    
    [Header("远程攻击设置")]
    public GameObject swordPre;
    public float speed = 20f;
    
    private SkillSystem skillSystem;
    private List<Button> allSkillButtons = new List<Button>();

    void Start()
    {
        skillSystem = FindObjectOfType<SkillSystem>();
        if (skillSystem == null)
        {
            GameObject obj = new GameObject("SkillSystem");
            skillSystem = obj.AddComponent<SkillSystem>();
        }

        // 初始化技能系统
        InitializeSkills();
        
        // 收集所有技能按钮
        allSkillButtons.Add(lockHP);
        allSkillButtons.Add(spawn);
        allSkillButtons.Add(farAttack);
        allSkillButtons.Add(renew);
        allSkillButtons.Add(platformCard);
        
        // 绑定按钮事件
        lockHP.onClick.AddListener(() => SelectSkill("LockHP"));
        spawn.onClick.AddListener(() => SelectSkill("Spawn"));
        renew.onClick.AddListener(() => SelectSkill("Renew"));
        platformCard.onClick.AddListener(() => SelectSkill("Platform"));
        farAttack.onClick.AddListener(() => SelectSkill("FarAttack"));
    }

    void InitializeSkills()
    {
        // 清空现有技能
        skillSystem.skills.Clear();

        // 添加锁血技能
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "LockHP",
            triggerKey = KeyCode.Q,
            cooldown = 10f,
            onExecute = () => 
            {
                if (lifeSystem != null)
                    lifeSystem.obstacleHurts = 0;
                Debug.Log("锁血技能触发！");
            }
        });

        // 添加复活技能
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "Spawn",
            triggerKey = KeyCode.E,
            cooldown = 30f,
            onExecute = () => 
            {
                if (CanRespawn())
                    RespawnPlayer();
                Debug.Log("复活技能触发！");
            }
        });

        // 添加恢复技能
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "Renew",
            triggerKey = KeyCode.R,
            cooldown = 20f,
            onExecute = () => 
            {
                if (lifeSystem != null)
                    lifeSystem.playerLifeLevel *= 1.1f;
                Debug.Log("恢复技能触发！");
            }
        });

        // 添加平台技能
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "Platform",
            triggerKey = KeyCode.F,
            cooldown = 15f,
            onExecute = () => 
            {
                if (platform != null)
                    platform.gapTime += 10f;
                Debug.Log("平台技能触发！");
            }
        });

        // 添加远程攻击技能
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "FarAttack",
            triggerKey = KeyCode.T,
            cooldown = 3f,
            onExecute = () => 
            {
                ExecuteFarAttack();
                Debug.Log("远程攻击技能触发！");
            }
        });
    }

    void ExecuteFarAttack()
    {
        if (swordPre != null)
        {
            // 获取玩家对象
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                // 计算生成位置（玩家位置 + 右侧偏移）
                Vector3 pos = player.transform.position + player.transform.right * 2;
                GameObject newBullet = Instantiate(swordPre, pos, player.transform.rotation);
                
                // 剑气移动
                Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = player.transform.right * speed;
                }
                
                // 3秒后自动销毁
                Destroy(newBullet, 3f);
            }
        }
    }

    void SelectSkill(string skillName)
    {
        // 解锁选择的技能
        skillSystem.UnlockSkill(skillName);
        
        // 禁用所有其他按钮
        DisableAllButtons();
        
        Debug.Log($"已选择技能: {skillName}，按对应按键使用");
    }

    void DisableAllButtons()
    {
        foreach (Button button in allSkillButtons)
        {
            if (button != null)
            {
                button.interactable = false;
            }
        }
    }

    bool CanRespawn()
    {
        GameObject currentPlayer = GameObject.FindGameObjectWithTag("Player");
        LifeSystem playerLife = currentPlayer?.GetComponent<LifeSystem>();
        return currentPlayer == null || (playerLife != null && playerLife.playerLifeLevel <= 0);
    }

    void RespawnPlayer()
    {
        Vector3 respawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0, 2, 0);
        
        // 销毁旧玩家
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        if (oldPlayer != null) Destroy(oldPlayer);
        
        // 创建新玩家
        if (playerPrefab != null)
        {
            GameObject newPlayer = Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
            lifeSystem = newPlayer.GetComponent<LifeSystem>();
        }
    }
}