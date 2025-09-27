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

    // Start is called before the first frame update
    void Start()
    {

        enemy = Instantiate(enemyPrefab, enemyPrefab.transform.position, Quaternion.identity);


        head.onClick.AddListener(HeadCard);//按钮启动弱点机制
    }

    void HeadCard()
    {
        if (weakPointPrefab != null && enemy != null)
        {
            // 检查是否已经存在weakPoint，如果存在就先销毁
            if (weakPoint != null)
            {
                Destroy(weakPoint);
                weakPoint = null;
            }
        }
        if (weakPointPrefab != null && enemy != null)
            {

                Vector3 spawnPosition = enemy.transform.position + new Vector3(0, 2f, 0); // 任意单位到时候再调整
                weakPoint = Instantiate(weakPointPrefab, spawnPosition, Quaternion.identity);


            }

    }
    void Update()
    {
       
        timer += Time.deltaTime;
        if (timer > 10)
        {
            MakeObstacle();
            timer = 0;
            Debug.Log("进入计时器");

        }
        if (obstacle != null)
        {
            obstacle.transform.position  = Vector3.MoveTowards(obstacle.transform.position, endPoint.transform.position, obstacleSpeed * Time.deltaTime);
        }
    }
    void MakeObstacle()
    {
        obstacle = Instantiate(obstaclePrefabs, startPoint.transform.position, quaternion.identity);

    }
}