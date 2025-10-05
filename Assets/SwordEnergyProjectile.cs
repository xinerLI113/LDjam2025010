using UnityEngine;

public class SwordEnergyProjectile : MonoBehaviour
{
    [Header("剑气属性")]
    public int damage = 30;
    
    private Vector3 moveDirection;    // 移动方向
    private float moveSpeed;          // 移动速度
    private float maxDistance;        // 最大距离
    private Vector3 startPosition;    // 起始位置
    private bool isMoving = true;     // 是否在移动

    

    // 添加这个Setup方法！
    public void Setup(Vector3 direction, float speed, float maxDist, Vector3 startPos)
    {
        moveDirection = direction.normalized;
        moveSpeed = speed;
        maxDistance = maxDist;
        startPosition = startPos;
        
        Debug.Log($"剑气设置完成 - 方向: {moveDirection}, 速度: {moveSpeed}, 最大距离: {maxDistance}");
    }

    void Update()
    {
        if (!isMoving) return;

        // 独立移动剑气（不依赖玩家）
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

        // 检查飞行距离
        CheckDistance();
    }

    void CheckDistance()
    {
        float currentDistance = Vector3.Distance(startPosition, transform.position);
        
        if (currentDistance >= maxDistance)
        {
            DestroyProjectile();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // 忽略玩家和其他的剑气
        if (other.CompareTag("Player") || other.CompareTag("SwordEnergy")) return;

        // 对敌人造成伤害
        if (other.CompareTag("Enemy"))
        {
            AttackEnemy enemy = other.GetComponent<AttackEnemy>();
            if (enemy != null)
            {
                enemy.lifeLevel -= damage;
                Debug.Log($"剑气命中敌人，造成{damage}点伤害");
            }
        }

        // 播放击中特效
        

        // 销毁剑气
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        isMoving = false;
        
        // 停止轨迹特效
        
        Destroy(gameObject, 0.1f); // 稍微延迟确保特效播放
    }

    // 可视化调试
    
}