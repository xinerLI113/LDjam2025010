using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Button head;
    public GameObject weakPointPrefab;
    public GameObject enemyPrefab;
    private GameObject enemy;
    private GameObject weakPoint;
    public GameObject obstaclePrefabs;
    private GameObject obstacle;
    public GameObject startPoint;
    public GameObject endPoint;
    public float obstacleSpeed = 3;
    private float timer;
    public float enemytimer;

    [Header("Y轴随机范围")]
    public float yOffsetRange = 2f;
    
    public int obstacleInterval = 5;

    private Vector3 targetPosition;
    private bool hasObstacle = false;

    void Start()
    {
        head.onClick.AddListener(HeadCard);
    }

    void HeadCard()
    {
        if (weakPointPrefab != null && enemy != null)
        {
            if (weakPoint != null)
            {
                Destroy(weakPoint);
                weakPoint = null;
            }
        }
        if (weakPointPrefab != null && enemy != null)
        {
            Vector3 spawnPosition = enemy.transform.position + new Vector3(0, 2f, 0);
            weakPoint = Instantiate(weakPointPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void Update()
    {
        enemytimer += Time.deltaTime;
        timer += Time.deltaTime;
        
        if (timer > obstacleInterval && !hasObstacle)
        {
            MakeObstacle();
            timer = 0;
        }
        
        if (obstacle != null)
        {
            // 移动障碍物
            obstacle.transform.position = Vector3.MoveTowards(
                obstacle.transform.position,
                targetPosition,
                obstacleSpeed * Time.deltaTime
            );

            // 检查是否到达目标
            if (Vector3.Distance(obstacle.transform.position, targetPosition) <= 0.01f)
            {
                
                DestroyImmediate(obstacle);
                obstacle = null;
                hasObstacle = false;
            }
        }
        
        if (enemytimer >= 50)
        {
            enemy = Instantiate(enemyPrefab, enemyPrefab.transform.position, Quaternion.identity);
            enemytimer = 0;
        }
    }

    void MakeObstacle()
    {
        

        // 生成随机Y偏移
        float randomYOffset = UnityEngine.Random.Range(-yOffsetRange, yOffsetRange);

        // 起始位置
        Vector3 spawnPosition = startPoint.transform.position;
        spawnPosition.y += randomYOffset;

        // 目标位置
        targetPosition = endPoint.transform.position;
        targetPosition.y += randomYOffset;

        obstacle = Instantiate(obstaclePrefabs, spawnPosition, Quaternion.identity);
        hasObstacle = true;
        
        
    }

    // 在Scene视图中可视化目标位置
    
}