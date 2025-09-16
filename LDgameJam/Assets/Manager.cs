using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Button head;
    public GameObject weakPointPrefab;  // 预制体引用
    public GameObject enemyPrefab; 
    private GameObject enemy;
    private GameObject weakPoint;

    // Start is called before the first frame update
    void Start()
    {
        
        enemy = Instantiate(enemyPrefab, enemyPrefab.transform.position, Quaternion.identity);
        
        
        head.onClick.AddListener(HeadCard);
    }

    void HeadCard()
    {
       
        if (weakPointPrefab != null && enemy != null)
        {
            
            Vector3 spawnPosition = enemy.transform.position + new Vector3(0, 2f, 0); // 任意单位到时候再调整
            weakPoint = Instantiate(weakPointPrefab, spawnPosition, Quaternion.identity);
            
            
        }
        
    }
}