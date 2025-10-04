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
    public GameObject startPoint;  // 起始空物体
    public GameObject endPoint;    // 终点空物体
    public float obstacleSpeed = 3;
    private float timer;
    public float enemytimer;

    [Header("Y轴随机范围")]
    public float yOffsetRange = 2f;  // Y轴随机偏移范围

    
    public int obstacleInterval = 5;

    // 记录障碍物的目标位置（包含随机Y值）
    private Vector3 targetPosition;

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
        if (timer > obstacleInterval)
        {
            MakeObstacle();
            timer = 0;
            Debug.Log("进入计时器");
        }
        if (obstacle != null)
        {
            // 移动到包含随机Y值的目标位置
            obstacle.transform.position = Vector3.MoveTowards(
                obstacle.transform.position,
                targetPosition,
                obstacleSpeed * Time.deltaTime
            );
        }
        if (enemytimer >= 50)
        {
            enemy = Instantiate(enemyPrefab, enemyPrefab.transform.position, Quaternion.identity);
            enemytimer = 0;
        }
    }

    void MakeObstacle()
    {
        if (startPoint == null || endPoint == null) return;

        // 生成随机Y偏移
        float randomYOffset = UnityEngine.Random.Range(-yOffsetRange, yOffsetRange);

        // 起始位置（包含随机Y值）
        Vector3 spawnPosition = startPoint.transform.position;
        spawnPosition.y += randomYOffset;

        // 目标位置（保持相同的随机Y值，只改变X和Z）
        targetPosition = endPoint.transform.position;
        targetPosition.y += randomYOffset;  // 终点也使用相同的Y偏移

        obstacle = Instantiate(obstaclePrefabs, spawnPosition, quaternion.identity);


    }

    
}