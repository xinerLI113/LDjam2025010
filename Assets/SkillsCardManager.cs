using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsCardManager : MonoBehaviour
{
    public Button spawn;           // 复活
    public Button farAttack;       // 剑气攻击
    public Button renew;           // 恢复
    public Button mechBody;        // 机甲身体
    
    [Header("引用")]
    public GameObject playerPrefab;
    public Transform spawnPoint;
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

        InitializeSkills();
        
        // 收集按钮
        allSkillButtons.Add(spawn);
        allSkillButtons.Add(farAttack);
        allSkillButtons.Add(renew);
        allSkillButtons.Add(mechBody);
        
        // 绑定事件
        spawn.onClick.AddListener(() => SelectSkill("Spawn"));
        farAttack.onClick.AddListener(() => SelectSkill("FarAttack"));
        renew.onClick.AddListener(() => SelectSkill("Renew"));
        mechBody.onClick.AddListener(() => SelectSkill("MechBody"));
    }

    void InitializeSkills()
    {
        skillSystem.skills.Clear();

        // 复活技能 - CD 30秒
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "Spawn",
            triggerKey = KeyCode.Q,
            cooldown = 30f,
            onExecute = () => 
            {
                RespawnPlayer();
                Debug.Log("复活技能触发！");
            }
        });

        // 剑气攻击 - CD 3秒
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "FarAttack",
            triggerKey = KeyCode.E,
            cooldown = 3f,
            onExecute = () => 
            {
                ExecuteFarAttack();
                Debug.Log("剑气攻击触发！");
            }
        });

        // 恢复技能 - CD 20秒
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "Renew",
            triggerKey = KeyCode.R,
            cooldown = 20f,
            onExecute = () => 
            {
                // 这里添加恢复逻辑
                Debug.Log("恢复技能触发！");
            }
        });

        // 机甲身体 - CD 15秒
        skillSystem.skills.Add(new SkillSystem.SkillData
        {
            skillName = "MechBody",
            triggerKey = KeyCode.F,
            cooldown = 15f,
            onExecute = () => 
            {
                // 这里添加机甲身体逻辑
                Debug.Log("机甲身体技能触发！");
            }
        });
    }

    void ExecuteFarAttack()
    {
        if (swordPre != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 pos = player.transform.position + player.transform.right * 2;
                GameObject newBullet = Instantiate(swordPre, pos, player.transform.rotation);
                
                Rigidbody2D rb = newBullet.GetComponent<Rigidbody2D>();
                if (rb != null) rb.velocity = player.transform.right * speed;
                
                Destroy(newBullet, 3f);
            }
        }
    }

    void SelectSkill(string skillName)
    {
        skillSystem.UnlockSkill(skillName);
        DisableAllButtons();
        Debug.Log($"已选择技能: {skillName}");
    }

    void DisableAllButtons()
    {
        foreach (Button button in allSkillButtons)
        {
            if (button != null) button.interactable = false;
        }
    }

    void RespawnPlayer()
    {
        Vector3 respawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0, 2, 0);
        
        GameObject oldPlayer = GameObject.FindGameObjectWithTag("Player");
        if (oldPlayer != null) Destroy(oldPlayer);
        
        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
        }
    }
}