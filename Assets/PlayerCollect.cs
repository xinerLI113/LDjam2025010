using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollect : MonoBehaviour
{
    public int materialCount;
    private Rigidbody2D rb;
    public string sceneToLoad = "BossRoom";
    public bool platFormEnable = false;
    
    [Header("入口设置")]
    public GameObject enterPrefab; // 拖入入口预制体
    public float spawnDistance = 2f; // 生成距离
    private GameObject currentEnter; // 当前入口对象

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 当收集够材料时生成入口
        if (materialCount >= 7 && currentEnter == null)
        {
            SpawnEnterInFront();
        }
    }

    void SpawnEnterInFront()
    {
        if (enterPrefab != null)
        {
            // 计算玩家前方的位置
            Vector3 spawnPosition = transform.position + transform.right * spawnDistance;
            
            // 生成入口
            currentEnter = Instantiate(enterPrefab, spawnPosition, Quaternion.identity);
            Debug.Log("入口生成在玩家前方");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Material"))
        {
            Destroy(collision.gameObject);
            materialCount++;
        }
        if (collision.gameObject.CompareTag("Enter"))
        {
            SceneManager.LoadScene("BossRoom");
        }
        if (materialCount >= 7)
        {
            platFormEnable = true;
        }
    }
}