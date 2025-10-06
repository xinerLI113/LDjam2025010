using UnityEngine;

namespace Script
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class KnifeSlashBehaviour : MonoBehaviour
    {
        [Header("Effects")] [Tooltip("Prefab to instantiate when the knife hits something.")] [SerializeField]
        private GameObject hitEffectPrefab;

        // Private State
        private Rigidbody2D rb;
        private float _damage;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 0;
            rb.freezeRotation = true;
        }

        /// <summary>
        /// Initializes and launches the projectile.
        /// </summary>
        /// <param name="direction">The horizontal direction to travel (-1 for left, 1 for right).</param>
        /// <param name="speed">How fast to travel.</param>
        /// <param name="lifetime">How long the projectile will exist before being destroyed.</param>
        /// <param name="damage">The amount of damage to deal on hit.</param>
        public void Launch(float direction, float speed, float lifetime, float damage)
        {
            this._damage = damage;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y,
                transform.localScale.z);
            
            rb.velocity = new Vector2(direction * speed, 0);
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player"))
            {
                if (other.TryGetComponent<LifeSystem>(out var enemyHealth))
                {
                    enemyHealth.TakeDamage(_damage);
                }

                HandleImpact();
            }
        }

        private void HandleImpact()
        {
            rb.velocity = Vector2.zero;

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}