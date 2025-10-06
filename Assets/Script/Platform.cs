using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject platFormPrefab;
    GameObject platForm;
    public Vector2 gap = new Vector2(2, 2);
    public float gapTime = 10;
    [SerializeField] private PlayerCollect playerCollect;
    [SerializeField] private PlatformManager platformManager;
    [SerializeField] private AttackEnemy attackEnemy;

    void Start()
    {
        playerCollect = FindFirstObjectByType<PlayerCollect>();
        attackEnemy = FindFirstObjectByType<AttackEnemy>();
        
        // 确保一开始伤害为0
        if (attackEnemy != null)
        {
            attackEnemy.SetDamageEnabled(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && platformManager.platFormEnable == true)
        {
            platForm = Instantiate(platFormPrefab, gap, quaternion.identity);
            playerCollect.materialCount = 0;
        }
        
        if (Time.time > gapTime && platForm != null)
        {
            Destroy(platForm);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // 站在平台上时启用伤害
            if (attackEnemy != null)
            {
                attackEnemy.SetDamageEnabled(true);
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            // 离开平台时禁用伤害
            if (attackEnemy != null)
            {
                attackEnemy.SetDamageEnabled(false);
            }
        }
    }
}