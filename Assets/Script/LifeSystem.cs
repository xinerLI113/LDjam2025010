using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Script
{
    public class LifeSystem : MonoBehaviour
    {
        [SerializeField] private float playerLifeLevel = 3;
        public int obstacleHurts = 1;
        public float playerWaterValue = 20;
        public bool isInWater = false;
        public UnityEvent<float, float> onPlayerDeath, onPlayerHealthIncrement, onPlayerHealthDecrement; // float 1 is NewValue, float 2 is OldValue

        public bool IsInvincible { get; private set; } = false;

        public float HealthLevel
        {
            get
            {
                return playerLifeLevel;
            }
            private set
            {
                playerLifeLevel = value;
            }
        }

        [Header("检测速度控制")]
        public float waterCheckInterval = 0.5f; // 每0.5秒检测一次
        private float waterTimer = 0f;

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                TakeDamage(obstacleHurts);
            }
        
            if (collision.gameObject.CompareTag("Water"))
            {
                isInWater = true;
            }
        }
    
        void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Water"))
            {
                isInWater = false;
            }
        }

        void Update()
        {
            // 使用计时器控制检测频率
            waterTimer += Time.deltaTime;
            if (waterTimer >= waterCheckInterval)
            {
                UpdateWaterValue();
                waterTimer = 0f;
            }
            if (playerWaterValue <= 0)
            {
                Destroy(gameObject);
            }
        }
    
        void UpdateWaterValue()
        {
            if (isInWater)
            {
                playerWaterValue--; // 在水里减少
            }
            else
            {
                playerWaterValue++; // 不在水里恢复
            }
        
            playerWaterValue = Mathf.Clamp(playerWaterValue, 0, 100);
        }

        public void TakeDamage(float damage)
        {
            if (IsInvincible) return;
            float oldHealth = HealthLevel;
            HealthLevel -= damage;
            if (HealthLevel <= 0)
            {
                HealthLevel = 0;
                onPlayerDeath.Invoke(HealthLevel, oldHealth);
                Destroy(gameObject);
            }
            else
            {
                onPlayerHealthDecrement.Invoke(HealthLevel, oldHealth);
            }
        }

        public void AddHealth(float amount)
        {
            onPlayerHealthIncrement.Invoke(HealthLevel + amount, HealthLevel);
            HealthLevel += amount;
        }
    }
}