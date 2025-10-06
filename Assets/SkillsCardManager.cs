using System.Collections;
using System.Collections.Generic;
using Script;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SkillsCardManager : MonoBehaviour
{
    public Button lockHP;
    public Button spawn;
    public Button farAttack;
     
    public GameObject playerPrefab; 
    public Transform spawnPoint;
    public Button renew;
    public Button platformCard;
    
    [SerializeField] private LifeSystem lifeSystem;
    [SerializeField] private Platform platform;
    public int randomNumber;
    private bool isInitialized = false;
    private GameObject currentPlayer;
    private bool isRenew = false;
    private float timer;
    private bool platformCardEnable=false;



    void Start()
    {
        InitializeReferences();
        if(lockHP!=null)
        lockHP.onClick.AddListener(LockCard);
        if(spawn!=null)
        spawn.onClick.AddListener(SpawnCard);
        if(renew!=null)
        renew.onClick.AddListener(AddHP);
        if(platformCard!=null)
        platformCard.onClick.AddListener(PlatformCard);
        
    }
    void PlatformCard()
    {
        platformCardEnable = true;
    }
    void AddHP()
    {
        isRenew = true;
        timer = 20;
    }

    void InitializeReferences()
    {
        
        currentPlayer = GameObject.FindGameObjectWithTag("Player");
        
        if (currentPlayer != null)
        {
            lifeSystem = currentPlayer.GetComponent<LifeSystem>();
            if (lifeSystem != null)
            {
                isInitialized = true;
               
            }
        }

        
    }

    void Update()
    {
        timer += Time.deltaTime;


        // 如果玩家被销毁，重新查找
        if (currentPlayer == null && isInitialized)
        {
            isInitialized = false;
            InitializeReferences();
            return;
        }

        if (!isInitialized) return;

        // 锁血逻辑
        if (lifeSystem.playerLifeLevel <= 1 && randomNumber == 1)
        {
            lifeSystem.obstacleHurts = 0;

        }
        if (isRenew == true && timer >= 20)
        {

            lifeSystem.playerLifeLevel *= 1.1f;
            timer = 0;//每二十秒恢复生命值,药神
        }
        if (platformCardEnable)
        {
            
            platform.gapTime += 10;
            platformCardEnable = false;
        }
        
        
    }

    void LockCard()
    {    
        randomNumber = UnityEngine.Random.Range(1, 10);
    }

    void SpawnCard()
    {
        
        if (CanRespawn())
        {
            RespawnPlayer();
        }
    }

    bool CanRespawn()
    {
        if (currentPlayer == null) return true;
        if (lifeSystem != null && lifeSystem.playerLifeLevel <= 0) return true;
        
        return false;
    }

    void RespawnPlayer()
    {
        try
        {
           
            if (currentPlayer != null && lifeSystem != null && lifeSystem.playerLifeLevel <= 0)
            {
                Destroy(currentPlayer);
                currentPlayer = null;
                lifeSystem = null;
            }

            Vector3 respawnPosition = GetRespawnPosition();
            
            // 创建新玩家
            if (playerPrefab != null)
            {
                currentPlayer = Instantiate(playerPrefab, respawnPosition, Quaternion.identity);
                lifeSystem = currentPlayer.GetComponent<LifeSystem>();
                
                if (lifeSystem != null)
                {
                    lifeSystem.playerLifeLevel *= 0.7f;
                    isInitialized = true;
                   
                }
                
            }
            
        }
        catch (System.Exception)
        {
    
            currentPlayer = null;
            lifeSystem = null;
            isInitialized = false;
        }
    }

    Vector3 GetRespawnPosition()
    {
        if (spawnPoint != null)
        {
            return spawnPoint.position;
        }
        
        // 使用默认安全位置
        return new Vector3(0, 2, 0);
    }
}