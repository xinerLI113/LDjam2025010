using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapLoop : MonoBehaviour
{
    [System.Serializable]
    public class SceneSegment
    {
        public GameObject segmentPrefab;
        public float length = 10f; // 手动设置长度，避免计算错误
        public Vector3 connectionPoint;
    }
    
    [Header("Tilemap设置")]
    public Tilemap tilemap;
    public SceneSegment[] segments;
    public Transform cameraTransform;
    
    [Header("循环设置")]
    public int tileWidthInCells = 10; // 手动指定单元格数量
    
    private int currentHeadIndex = 0;
    private float tileWidth;
    
    void Start()
    {
        // 安全检查
        if (tilemap == null)
        {
            tilemap = GetComponent<Tilemap>();
            if (tilemap == null)
            {
                Debug.LogError("没有找到Tilemap组件！");
                return;
            }
        }
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        // 计算tile宽度
        tileWidth = tilemap.cellSize.x * tileWidthInCells;
        Debug.Log($"Tile宽度: {tileWidth}, 单元格大小: {tilemap.cellSize.x}");
        
        // 初始生成片段
        for (int i = 0; i < 3; i++)
        {
            PlaceTile(i);
        }
    }
    
    void Update()
    {
        if (tilemap == null || cameraTransform == null) return;
        
        // 检测是否需要循环
        if (cameraTransform.position.x > GetCurrentTileRightEdge())
        {
            RecycleOldestTile();
            PlaceNewTileOnRight();
            currentHeadIndex++;
        }
    }
    
    void PlaceTile(int index)
    {
        if (segments == null || segments.Length == 0)
        {
            Debug.LogError("没有设置segments数组！");
            return;
        }
        
        int segmentIndex = index % segments.Length;
        if (segments[segmentIndex] == null || segments[segmentIndex].segmentPrefab == null)
        {
            Debug.LogError($"Segment {segmentIndex} 没有设置prefab！");
            return;
        }
        
        Vector3 spawnPosition = CalculateSpawnPosition(index);
        GameObject newSegment = Instantiate(segments[segmentIndex].segmentPrefab, spawnPosition, Quaternion.identity);
        newSegment.name = $"Segment_{index}";
    }
    
    void RecycleOldestTile()
    {
        // 找到并销毁最旧的segment
        string oldestName = $"Segment_{currentHeadIndex}";
        GameObject oldest = GameObject.Find(oldestName);
        if (oldest != null)
        {
            Destroy(oldest);
        }
    }
    
    void PlaceNewTileOnRight()
    {
        int newIndex = currentHeadIndex + 3; // 在右边放置新tile
        PlaceTile(newIndex);
    }
    
    Vector3 CalculateSpawnPosition(int index)
    {
        return new Vector3(index * tileWidth, 0, 0);
    }
    
    float GetCurrentTileRightEdge()
    {
        return (currentHeadIndex + 1) * tileWidth * 0.7f; // 提前一些触发
    }
    
    // 修复后的方法 - 不再计算，直接使用手动设置
    int GetTileWidthInCells()
    {
        return Mathf.Max(1, tileWidthInCells); // 确保至少为1
    }
    
    // 在场景中可视化显示边界
    void OnDrawGizmos()
    {
        if (tilemap == null) return;
        
        Gizmos.color = Color.red;
        for (int i = currentHeadIndex; i < currentHeadIndex + 3; i++)
        {
            Vector3 position = CalculateSpawnPosition(i);
            Gizmos.DrawWireCube(position + Vector3.right * tileWidth * 0.5f, 
                               new Vector3(tileWidth, 5f, 1f));
        }
        
        // 标记当前右边界
        if (cameraTransform != null)
        {
            Gizmos.color = Color.yellow;
            float rightEdge = GetCurrentTileRightEdge();
            Gizmos.DrawLine(new Vector3(rightEdge, -10, 0), new Vector3(rightEdge, 10, 0));
        }
    }
}