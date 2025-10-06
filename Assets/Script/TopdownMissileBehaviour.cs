using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class TopdownMissileBehaviour : MonoBehaviour
    {
        [Header("Phase Settings")]
        [Tooltip("Duration of aiming phase (marker visible)")]
        public float aimDuration = 1.5f;
        
        [Tooltip("Height above target where missile spawns")]
        public float missileHeight = 10f;
        
        [Header("Missile Settings")]
        [Tooltip("Downward speed of the missile")]
        public float missileSpeed = 15f;
        
        [Tooltip("Use gravity for missile")]
        public bool useGravity = true;
        
        [Tooltip("Gravity scale if using gravity")]
        public float gravityScale = 3f;
        
        [Header("Visual Prefabs")]
        [Tooltip("Ground marker prefab shown during aiming")]
        public GameObject markerPrefab;
        
        [Tooltip("Missile object prefab")]
        public GameObject missilePrefab;
        
        [Tooltip("Explosion effect prefab")]
        public GameObject explosionPrefab;
        
        [Header("Explosion Settings")]
        [Tooltip("How long explosion effect lasts")]
        public float explosionDuration = 2f;
        
        [Tooltip("Explosion damage radius (optional)")]
        public float explosionRadius = 2f;
        
        [Tooltip("Explosion damage")]
        public float explosionDamage = 10f;
        
        // Private variables
        private GameObject markerInstance;
        private GameObject missileInstance;
        private Vector3 targetPosition;
        
        void Start()
        {
            targetPosition = transform.position;
            StartCoroutine(MissileSequence());
        }
        
        private IEnumerator MissileSequence()
        {
            // PHASE 1: Aiming Phase
            yield return StartCoroutine(AimingPhase());
            
            // PHASE 2: Fire Phase
            yield return StartCoroutine(FirePhase());
            
            // Clean up this controller object
            Destroy(gameObject);
        }
        
        private IEnumerator AimingPhase()
        {
            // Spawn marker at target position
            if (markerPrefab != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.down, 100f);
                markerInstance = Instantiate(markerPrefab, hit.point, Quaternion.identity);
            }
            
            // Wait for aim duration
            yield return new WaitForSeconds(aimDuration);
            
            // Remove marker
            if (markerInstance != null)
            {
                Destroy(markerInstance);
            }
        }
        
        private IEnumerator FirePhase()
        {
            // Spawn missile above the target position
            if (missilePrefab != null)
            {
                Vector3 missileSpawnPos = targetPosition + Vector3.up * missileHeight;
                missileInstance = Instantiate(missilePrefab, missileSpawnPos, Quaternion.Euler(0, 0, 0));
                
                // Set up missile physics
                Rigidbody2D missileRb = missileInstance.GetComponent<Rigidbody2D>();
                if (missileRb != null)
                {
                    if (useGravity)
                    {
                        missileRb.gravityScale = gravityScale;
                    }
                    else
                    {
                        missileRb.gravityScale = 0;
                    }
                    
                    // Apply initial downward velocity
                    missileRb.velocity = Vector2.down * missileSpeed;
                }
                
                // Add collision handler to missile
                MissileCollisionHandler collisionHandler = missileInstance.GetComponent<MissileCollisionHandler>();
                if (collisionHandler == null)
                {
                    collisionHandler = missileInstance.AddComponent<MissileCollisionHandler>();
                }
                collisionHandler.Initialize(this);
            }
            
            // Wait until missile is destroyed (collision will handle this)
            while (missileInstance != null)
            {
                yield return null;
            }
        }
        
        public void OnMissileImpact(Vector3 impactPosition)
        {
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, impactPosition, Quaternion.identity);
                Destroy(explosion, explosionDuration);
                
                DealExplosionDamage(impactPosition);
            }
        }
        
        private void DealExplosionDamage(Vector3 explosionCenter)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(explosionCenter, explosionRadius);
            
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.TryGetComponent(out LifeSystem lifeSystem))
                {
                    lifeSystem.TakeDamage(explosionDamage);
                    Debug.Log("Player hit by explosion!");
                }
            }
        }
        
        // Visualize explosion radius in editor
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}