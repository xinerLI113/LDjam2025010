using System.Collections;
using System.Collections.Generic;
using Script;
using Script.Player;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    public enum BossState
    {
        Idle,
        Attacking
    }

    [SerializeField] private float idleStateDuration = 3f;
    [SerializeField] private float postAttackStateDuration = 1f;

    [Space, Header("Ability 1 Settings")] 
    [SerializeField] private int ability1AttackCount = 3;
    [SerializeField] private float ability1AttackIntervalDuration = 1f;
    [SerializeField] private float ability1AttackAimingDuration = 1f;
    [SerializeField] private GameObject ability1AttackPrefab;

    [Space, Header("Ability 2 Settings")] 
    [SerializeField] private int ability2AttackCount = 10;
    [SerializeField] private float ability2AttackIntervalDuration = 0.1f;
    [SerializeField] private float ability2Speed = 3f;
    [SerializeField] private float ability2RandomSpread = 3f;
    [SerializeField] private GameObject ability2AttackPrefab;
    
    private BossState _state = BossState.Idle;
    private float _currentStateDuration;
    private PlayerController _player;
    private bool _isAttacking = false; // Flag to prevent multiple simultaneous attacks

    void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _currentStateDuration = idleStateDuration; // Initialize properly
    }

    void FixedUpdate()
    {
        // Decrement state timer
        if (_currentStateDuration > 0)
        {
            _currentStateDuration -= Time.fixedDeltaTime;
        }

        // State machine
        switch (_state)
        {
            case BossState.Idle:
                if (_currentStateDuration <= 0 && !_isAttacking)
                {
                    StartCoroutine(PerformAttack());
                }
                break;
                
            case BossState.Attacking:
                // Just waiting for attack coroutine to finish
                break;
        }
    }

    private IEnumerator PerformAttack()
    {
        _isAttacking = true;
        _state = BossState.Attacking;
        
        if (_player == null)
            _player = FindObjectOfType<PlayerController>();

        int selectedAttack = Random.Range(0, 2); // Returns 0 or 1

        if (selectedAttack == 0)
        {
            yield return StartCoroutine(InstantiateAttack1(ability1AttackIntervalDuration, ability1AttackCount));
        }
        else
        {
            yield return StartCoroutine(InstantiateAttack2(ability2AttackIntervalDuration, ability2AttackCount));
        }

        // Post-attack delay
        yield return new WaitForSeconds(postAttackStateDuration);

        // Return to idle state
        _state = BossState.Idle;
        _currentStateDuration = idleStateDuration; // Reset timer
        _isAttacking = false;
    }

    private IEnumerator InstantiateAttack1(float duration, int times)
    {
        for (int i = 0; i < times; i++)
        {
            if (_player != null)
            {
                Vector3 targetPosition = _player.transform.position;
            
                GameObject missile = Instantiate(ability1AttackPrefab, targetPosition, Quaternion.identity);
            
                TopdownMissileBehaviour missileBehaviour = missile.GetComponent<TopdownMissileBehaviour>();
                if (missileBehaviour != null)
                {
                    missileBehaviour.aimDuration = ability1AttackAimingDuration;
                }
            }
            else
            {
                Debug.LogWarning("Player not found!");
            }

            if (i < times - 1)
            {
                yield return new WaitForSeconds(duration);
            }
        }
    }

    private IEnumerator InstantiateAttack2(float duration, int times)
    {
        for (int i = 0; i < times; i++)
        {
            if (_player != null)
            {
                Vector2 directionToPlayer = (_player.transform.position - transform.position).normalized;
            
                float randomAngle = Random.Range(-ability2RandomSpread, ability2RandomSpread);
                Vector2 randomizedDirection = RotateVector(directionToPlayer, randomAngle);
            
                GameObject pellet = Instantiate(ability2AttackPrefab, transform.position, Quaternion.identity);
            
                AttackPelletBehaviour pelletBehaviour = pellet.GetComponent<AttackPelletBehaviour>();
                if (pelletBehaviour != null)
                {
                    pelletBehaviour.PropelTowards(ability2Speed, randomizedDirection);
                }
            }
            else
            {
                Debug.LogWarning("Player not found! Make sure the player has the 'Player' tag.");
            }

            // Wait before next pellet (except on last iteration)
            if (i < times - 1)
            {
                yield return new WaitForSeconds(duration);
            }
        }
    }

    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);
    
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }
}