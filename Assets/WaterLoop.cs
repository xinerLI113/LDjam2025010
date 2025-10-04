using UnityEngine;

public class WaterLoopAtPrefabPosition : MonoBehaviour
{
    [Header("水预制体设置")]
    public GameObject waterPrefab;      // 你的水预制体
    public int waterSegmentCount = 3;   // 同时存在的水段数量
    public float waterSegmentLength = 10f; // 每个水段的长度
    
    [Header("玩家引用")]
    public Transform player;
    
    private GameObject[] waterSegments;
    private int currentHeadIndex = 0;
    private float totalWaterLength;
    private Vector3 basePosition; // 保存预制体的原始位置
    
    void Start()
    {
        // 自动查找玩家
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        
        // 保存预制体的原始位置
        basePosition = waterPrefab.transform.position;
        
        // 计算总长度
        totalWaterLength = waterSegmentLength * waterSegmentCount;
        
        // 初始化水段 - 使用预制体的位置作为基准
        waterSegments = new GameObject[waterSegmentCount];
        for (int i = 0; i < waterSegmentCount; i++)
        {
            Vector3 spawnPosition = basePosition + new Vector3(i * waterSegmentLength, 0, 0);
            waterSegments[i] = Instantiate(waterPrefab, spawnPosition, Quaternion.identity);
            waterSegments[i].name = $"WaterSegment_{i}";
        }
    }
    
    void Update()
    {
        if (player == null) return;
        
        // 检查玩家是否超出当前水段
        if (player.position.x > waterSegments[currentHeadIndex].transform.position.x + waterSegmentLength)
        {
            MoveWaterSegment();
        }
    }
    
    void MoveWaterSegment()
    {
        // 将最左边的水段移动到最右边
        GameObject segmentToMove = waterSegments[currentHeadIndex];
        Vector3 newPosition = segmentToMove.transform.position;
        newPosition.x += totalWaterLength;
        segmentToMove.transform.position = newPosition;
        
        // 更新索引
        currentHeadIndex = (currentHeadIndex + 1) % waterSegmentCount;
    }
}