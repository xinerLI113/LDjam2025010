using UnityEngine;

namespace Script
{
    public class AttackPelletBehaviour : MonoBehaviour
    {
        [Header("Movement")]
        private Vector2 direction;
        private float speed;
        private Rigidbody2D rb;
        
        [Header("Lifetime Settings")]
        [Tooltip("Time before pellet destroys itself (0 = infinite)")]
        public float lifetime = 5f;
        public float damage = 1f;
        
        [Tooltip("Destroy on collision with obstacles")]
        public bool destroyOnHit = true;
        
        private float spawnTime;
        
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            spawnTime = Time.time;
        }
        
        void Update()
        {
            // Handle lifetime destruction
            if (lifetime > 0 && Time.time - spawnTime >= lifetime)
            {
                Destroy(gameObject);
            }
        }
        
        void FixedUpdate()
        {
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
            else
            {
                transform.Translate(direction * speed * Time.fixedDeltaTime);
            }
        }
        
        /// <summary>
        /// Sets the pellet's direction and speed
        /// </summary>
        /// <param name="movementSpeed">Speed of the pellet</param>
        /// <param name="movementDirection">Normalized direction vector</param>
        public void PropelTowards(float movementSpeed, Vector2 movementDirection)
        {
            speed = movementSpeed;
            direction = movementDirection.normalized;
            
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy")) return;
            if (destroyOnHit)
            {
                if (collision.CompareTag("Player"))
                {
                    collision.gameObject.GetComponent<LifeSystem>().TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
    }
}