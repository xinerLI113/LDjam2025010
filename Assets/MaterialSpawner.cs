using UnityEngine;
using System.Collections.Generic;

public class SimpleMaterialSpawner : MonoBehaviour
{
    [Header("生成区域")]
    public Transform areaCenter;  
    public Vector3 spawnSize = new Vector3(10f, 5f, 10f);
    
    [Header("生成设置")]
    public GameObject collectiblePrefab;
    public int maxMaterials = 10;
    public float spawnInterval = 2f;
    
    private List<GameObject> spawnedMaterials = new List<GameObject>();
    private float spawnTimer = 0f;
    
    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnInterval && spawnedMaterials.Count < maxMaterials)
        {
            SpawnMaterial();
            spawnTimer = 0f;
        }
        

        for (int i = spawnedMaterials.Count - 1; i >= 0; i--)
        {
            if (spawnedMaterials[i] == null)
            {
                spawnedMaterials.RemoveAt(i);
            }
        }
    }
    
    void SpawnMaterial()
    {
        if (collectiblePrefab == null) return;
        
        //随机生成位置
        Vector3 randomPos = new Vector3(
            Random.Range(-spawnSize.x / 2, spawnSize.x / 2),
            Random.Range(-spawnSize.y / 2, spawnSize.y / 2),
            Random.Range(-spawnSize.z / 2, spawnSize.z / 2)
        );
        
        if (areaCenter != null)
        {
            randomPos += areaCenter.position;
        }
        else
        {
            randomPos += transform.position;
        }
        
        GameObject newMaterial = Instantiate(collectiblePrefab, randomPos, Quaternion.identity);
        spawnedMaterials.Add(newMaterial);
    }
    
    
    void OnDrawGizmos()
    {
        Vector3 center = areaCenter != null ? areaCenter.position : transform.position;
        
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, spawnSize);
        
        Gizmos.color = new Color(0, 1, 0, 0.1f);
        Gizmos.DrawCube(center, spawnSize);
    }
}