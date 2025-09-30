using UnityEngine;
using UnityEngine.UI;

public class MechBody : MonoBehaviour
{
    [Header("喷气飞行设置")]
    public float jumpForce = 10f;
    public float flyForce = 5f;
    public float maxFlyTime = 3f;
    public float jetDamage = 20f;
    public float jetDamageRadius = 2f;
    
    [Header("地面检测")]
    public float checkRadius = 0.2f;
    public LayerMask groundLayer;
    
    private Rigidbody2D rb;
    private bool isFlying = false;
    private bool isGrounded = false;
    private bool jetSkillEnabled = false;
    private float currentFlyTime = 0f;
    private float cooldownTimer = 10f;
    private bool isOnCooldown = false;
    public ParticleSystem jetParticles;
    public Button mechBodyBotton;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (jetParticles != null)
            jetParticles.Stop();
            
        mechBodyBotton.onClick.AddListener(ToggleJetSkill);
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;
        CheckGrounded();

        // 普通跳跃 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
        }

        // 喷气飞行
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jetSkillEnabled && !isOnCooldown)
        {
            StartJetFlight();
        }

        if (isFlying && Input.GetKey(KeyCode.Space) && currentFlyTime < maxFlyTime)
        {
            rb.AddForce(Vector2.up * flyForce);
            currentFlyTime += Time.deltaTime;
            ApplyJetDamage();
        }

        if (isFlying && (!Input.GetKey(KeyCode.Space) || currentFlyTime >= maxFlyTime))
        {
            StopJetFlight();
        }

        if (isOnCooldown && cooldownTimer >= 10f)
        {
            isOnCooldown = false;
        }
    }
    
    void CheckGrounded()
    {
        
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);
        
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPosition, checkRadius);
        isGrounded = false;
        
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != gameObject && collider.CompareTag("Ground"))
            {
                isGrounded = true;
                break;
            }
        }
        
        
        if (!isGrounded)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.6f);
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                isGrounded = true;
            }
        }
    }
    
    void ToggleJetSkill()
    {
        if (isOnCooldown)
        {
            
            return;
        }
        
        jetSkillEnabled = !jetSkillEnabled;
        
        
        if (!jetSkillEnabled && isFlying)
        {
            StopJetFlight();
        }
    }
    
    void StartJetFlight()
    {
        isFlying = true;
        currentFlyTime = 0f;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        
        if (jetParticles != null)
            jetParticles.Play();
            
        
    }
    
    void StopJetFlight()
    {
        isFlying = false;
        jetSkillEnabled = false;
        isOnCooldown = true;
        cooldownTimer = 0f;
        
        if (jetParticles != null)
            jetParticles.Stop();
    }
    
    void ApplyJetDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, jetDamageRadius);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))//这里伤害写的是灼烧的，后面看要不要再改
            {
                AttackEnemy enemyHealth = enemy.GetComponent<AttackEnemy>();
                if (enemyHealth != null)
                {
                    enemyHealth.lifeLevel -= jetDamage * Time.deltaTime;
                }
            }
        }
    }
    
    void OnDrawGizmos()
    {
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector2 checkPos = new Vector2(transform.position.x, transform.position.y - 0.5f);
        Gizmos.DrawWireSphere(checkPos, checkRadius);
        
        Gizmos.DrawRay(transform.position, Vector2.down * 0.6f);
    }
}