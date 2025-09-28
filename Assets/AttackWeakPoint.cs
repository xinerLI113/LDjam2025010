using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackWeakPoint : MonoBehaviour
{
    [SerializeField] private AttackEnemy attackEnemy;
    public float weakPointValue = 30;
    
    void Start()
    {
        
        if (attackEnemy == null)
        {
            attackEnemy = FindFirstObjectByType<AttackEnemy>();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.CompareTag("Skills"))
        {
            if (attackEnemy != null)
            {
                Debug.Log("进入碰撞");
                attackEnemy.lifeLevel -= weakPointValue;
            }
        }
    }
}