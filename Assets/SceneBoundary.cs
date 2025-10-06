using UnityEngine;

public class SceneBoundary : MonoBehaviour
{
    [Header("边界设置")]
    public float leftBound = -10f;
    public float rightBound = 10f;
    public float topBound = 5f;
    public float bottomBound = -5f;

    void Start()
    {
        CreateBoundaries();
    }

    void CreateBoundaries()
    {
        // 创建四个边界墙
        CreateBoundary("LeftBound", new Vector2(leftBound, 0), new Vector2(1, 20));
        CreateBoundary("RightBound", new Vector2(rightBound, 0), new Vector2(1, 20));
        CreateBoundary("TopBound", new Vector2(0, topBound), new Vector2(20, 1));
        CreateBoundary("BottomBound", new Vector2(0, bottomBound), new Vector2(20, 1));
    }

    void CreateBoundary(string name, Vector2 position, Vector2 size)
    {
        GameObject boundary = new GameObject(name);
        boundary.transform.position = position;
        
        // 添加碰撞体
        BoxCollider2D collider = boundary.AddComponent<BoxCollider2D>();
        collider.size = size;
    
        // 添加刚体并设置为静态
        Rigidbody2D rb = boundary.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Static;
        
        boundary.tag = "Boundary";
    }
}