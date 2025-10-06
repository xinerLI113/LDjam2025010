using UnityEngine;
using System.Collections; // Required for using coroutines (IEnumerator)

namespace Script.Player
{
    public class PlayerKnifeSlash : MonoBehaviour
    {
        [Header("Attack Settings")]
        [SerializeField] private KeyCode useKey = KeyCode.F;
        [SerializeField] private GameObject knifeSlashPrefab;
        [SerializeField] private float attackCooldown = 0.5f;

        [Header("Animation & Timing")]
        [Tooltip("The delay (in seconds) between playing the animation and the knife actually spawning.")]
        [SerializeField] private float attackDelay = 0.2f;

        [Header("Knife Projectile Stats")]
        [SerializeField] private float knifeSpeed = 20f;
        [SerializeField] private float knifeLifetime = 1.5f;
        [SerializeField] private float knifeDamage = 10f;

        [Header("Spawn Position")]
        [Tooltip("An optional empty GameObject childed to the player to define the spawn point.")]
        [SerializeField] private Transform spawnPoint;

        // Private State
        private PlayerController _playerController;
        private PlayerAnimatorController _scriptAnimator;
        private float _cooldownTimer;
        private float _lastFacingDirection = 1f;
        private bool _isAttacking = false;

        void Awake()
        {
            _playerController = GetComponent<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError("PlayerKnifeSlash requires a PlayerController component on the same object!");
                enabled = false;
            }
        }

        void Start()
        {
            _cooldownTimer = attackCooldown;
            _scriptAnimator = FindObjectOfType<PlayerAnimatorController>();
        }

        void Update()
        {
            _cooldownTimer += Time.deltaTime;
            
            if (Mathf.Abs(_playerController.InputDirection.x) > 0.1f)
            {
                _lastFacingDirection = Mathf.Sign(_playerController.InputDirection.x);
            }
            
            if (Input.GetKeyDown(useKey) && _cooldownTimer >= attackCooldown && !_isAttacking)
            {
                StartCoroutine(AttackSequence());
            }
        }

        /// <summary>
        /// A coroutine that handles the entire attack sequence: animation, delay, and projectile launch.
        /// </summary>
        private IEnumerator AttackSequence()
        {
            _isAttacking = true;
            _cooldownTimer = 0f;

            if (_scriptAnimator != null)
            {
                _scriptAnimator.TriggerUseAbility1();
            }
            
            yield return new WaitForSeconds(attackDelay);
            
            Vector3 position = (spawnPoint != null) ? spawnPoint.position : transform.position;
            GameObject knifeInstance = Instantiate(knifeSlashPrefab, position, Quaternion.identity);
            
            KnifeSlashBehaviour knifeBehaviour = knifeInstance.GetComponent<KnifeSlashBehaviour>();
            if (knifeBehaviour != null)
            {
                knifeBehaviour.Launch(_lastFacingDirection, knifeSpeed, knifeLifetime, knifeDamage);
            }
            else
            {
                Debug.LogWarning("The knifeSlashPrefab is missing the KnifeSlashBehaviour script!");
            }
            
            _isAttacking = false;
        }
    }
}