using UnityEngine;

public class SwordEnergy : MonoBehaviour
{
    [Header("剑气设置")]
    public GameObject swordEnergyPrefab;
    public float spawnDistance = 1.5f;
    public float moveSpeed = 15f;
    public float maxDistance = 20f;
    public KeyCode shootKey = KeyCode.F;

    void Update()
    {
        if (Input.GetKeyDown(shootKey))
        {
            CreateSwordEnergy();
        }
    }

    void CreateSwordEnergy()
    {
        if (swordEnergyPrefab == null) return;

        // 计算生成位置（玩家前方）
        Vector3 spawnPosition = transform.position + 
                               transform.forward * spawnDistance + 
                               transform.up * 0.5f;

        // 生成剑气，并获取剑气控制器
        GameObject swordEnergy = Instantiate(swordEnergyPrefab, spawnPosition, transform.rotation);
        
        // 设置剑气的移动方向和速度
        SwordEnergyProjectile projectile = swordEnergy.GetComponent<SwordEnergyProjectile>();
        if (projectile != null)
        {
            projectile.Setup(transform.forward, moveSpeed, maxDistance, spawnPosition);
        }
        else
        {
            Debug.LogError("剑气预制体缺少SwordEnergyProjectile脚本！");
        }
        
        Debug.Log("剑气发射！");
    }
}