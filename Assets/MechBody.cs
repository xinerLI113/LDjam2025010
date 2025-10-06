using Script;
using Script.Player;
using UnityEngine;
using UnityEngine.UI;

public class MechBody : MovementFeature
{
    [Header("Jetpack Settings")]
    [SerializeField] private float jetInitialBoost = 15f; // The initial "kick" when starting a flight
    [SerializeField] private float flyForce = 8f; // The sustained upward velocity while flying
    [SerializeField] private float maxFlyTime = 3f;
    [SerializeField] private float cooldownDuration = 10f;
    public ParticleSystem jetParticles;

    [Header("Jet Damage")]
    [SerializeField] private float jetDamage = 20f;
    [SerializeField] private float jetDamageRadius = 2f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("UI")]
    public Button mechBodyButton;

    // Private States
    private bool isFlying = false;
    private bool isOnCooldown = false;
    private float currentFlyTime = 0f;
    private float cooldownTimer = 0f;
    private PlayerAnimatorController scriptAnimator;
    private Rigidbody2D rb;

    void Start()
    {
        scriptAnimator = FindObjectOfType<PlayerAnimatorController>();
    }

    public override void Initialize(PlayerController controller)
    {
        base.Initialize(controller);

        if (jetParticles != null)
            jetParticles.Stop();

        if (mechBodyButton != null)
        {
            mechBodyButton.onClick.AddListener(ToggleJetSkill);
        }

        cooldownTimer = cooldownDuration;
    }

    public override void OnPreUpdate()
    {
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer >= cooldownDuration)
            {
                isOnCooldown = false;
                // Optional: Update UI to show skill is ready
            }
        }

        if (IsEnabled && !isFlying && !isOnCooldown && controller.IsGrounded && controller.IsJumpPressed())
        {
            StartJetFlight();
        }

        // Check conditions to STOP flying
        bool stopFlyingInput = IsEnabled && isFlying && !controller.IsJumpHeld();
        bool outOfFuel = isFlying && currentFlyTime >= maxFlyTime;
            
        if (stopFlyingInput || outOfFuel)
        {
            StopJetFlight();
        }
            
        // While flying, update timers and deal damage
        if (isFlying)
        {
            currentFlyTime += Time.deltaTime;
            ApplyJetDamage();
        }
    }

    public override Vector2 ModifyVelocity(Vector2 velocity)
    {
        if (isFlying)
            velocity.y = flyForce; 
            
        return velocity;
    }

    private void ToggleJetSkill()
    {
        if (isOnCooldown) return;

        IsEnabled = !IsEnabled;

        if (!IsEnabled && isFlying)
            StopJetFlight();
        
        // Optional: Update UI to show skill is on/off
    }

    private void StartJetFlight()
    {
        isFlying = true;
        currentFlyTime = 0f;
        scriptAnimator.SetJetActive(true);
        
        // Give an initial upward kick
        if (rb == null)
            rb = controller.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, jetInitialBoost);

        if (jetParticles != null)
            jetParticles.Play();
    }

    private void StopJetFlight()
    {
        scriptAnimator.SetJetActive(false);
        if (!isFlying) return;

        isFlying = false;
        IsEnabled = false;
        isOnCooldown = true;
        cooldownTimer = 0f;

        if (jetParticles != null)
            jetParticles.Stop();
    }

    private void ApplyJetDamage()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, jetDamageRadius, enemyLayer);
        foreach (Collider2D enemy in enemies)
        {
            if (enemy.TryGetComponent<LifeSystem>(out var enemyHealth))
                enemyHealth.TakeDamage(jetDamage * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, jetDamageRadius);
    }
}