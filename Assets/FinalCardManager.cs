using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;
using UnityEngine.UI;

public class FinalCardManager : MonoBehaviour
{
    [Header("按钮")]
    public Button respawnButton;
    
    [Header("设置")]
    public float respawnCooldown = 20f;
    public ParticleSystem respawnParticle;
    
    [Header("引用")]
    public GameObject playerPrefab;
    public Transform spawnPoint;
    
    private float respawnTimer = 0f;
    private bool respawnReady = true;
    public bool isSpawn;
    [SerializeField] private LifeSystem lifeSystem;
    public Button renew;

    void Start()
    {
        if (respawnParticle != null)
            respawnParticle.Stop();

        respawnButton.onClick.AddListener(OnRespawnButtonClick);
        UpdateButtonState();
        renew.onClick.AddListener(AddLifeLevel);
    }
    void AddLifeLevel()
    {
        lifeSystem.AddHealth(5);
    }

    void Update()
    {
        if (!respawnReady)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0f)
            {
                respawnReady = true;
                UpdateButtonState();
            }
        }
        
        // 每帧更新按钮状态
        UpdateButtonState();
    }

    void OnRespawnButtonClick()
    {
        if (respawnReady && IsPlayerDead())
        {
            // 移动粒子到重生位置并播放
            if (respawnParticle != null)
            {
                Vector3 respawnPos = spawnPoint != null ? spawnPoint.position : new Vector3(0, 2, 0);
                respawnParticle.transform.position = respawnPos;
                respawnParticle.Play();
            }
            
            RespawnPlayer();
            
            respawnReady = false;
            respawnTimer = respawnCooldown;
            UpdateButtonState();
        }
    }

    bool IsPlayerDead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        return player == null;
    }

    void RespawnPlayer()
    {
        Vector3 respawnPosition = spawnPoint != null ? spawnPoint.position : new Vector3(0, 2, 0);

        if (playerPrefab != null)
        {
            Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
            isSpawn = true;
           
        }
    }

    void UpdateButtonState()
    {
        bool isDead = IsPlayerDead();
        respawnButton.interactable = respawnReady && isDead;
        
        // 调试信息
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log($"玩家死亡: {isDead}, 技能就绪: {respawnReady}, 按钮可用: {respawnButton.interactable}");
        }
    }
}