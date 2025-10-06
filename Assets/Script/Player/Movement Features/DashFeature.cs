using Script.Player;
using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class DashFeature : MovementFeature
    {
        [Header("Dash Settings")]
        [SerializeField] private float dashSpeed = 30f;
        [SerializeField] private float dashDuration = 0.15f; // How long the high-speed movement lasts
        [SerializeField] private float dashCooldown = 1f;

        [Header("Options")]
        [Tooltip("Key to activate the dash.")]
        [SerializeField] private KeyCode dashKey = KeyCode.G;
        [Tooltip("The horizontal speed multiplier applied at the end of the dash.")]
        [SerializeField] private float endSpeedMultiplier = 0.5f;

        [Header("Effects")]
        [SerializeField] private ParticleSystem dashParticles;

        // Public State (for other scripts like PlayerHealth)
        public bool IsInvincible => isDashing;
        
        // Private State
        private bool isDashing;
        private float dashTimer;
        private float cooldownTimer;
        private Vector2 dashDirection;
        private float lastNonZeroDirection = 1f;
        private PlayerAnimatorController scriptAnimator;

        void Start()
        {
            scriptAnimator = FindObjectOfType<PlayerAnimatorController>();
        }

        public override void Initialize(PlayerController controller)
        {
            base.Initialize(controller);
            cooldownTimer = dashCooldown;
        }

        public override void OnPreUpdate()
        {
            // Update the last facing direction based on input
            if (Mathf.Abs(controller.InputDirection.x) > 0.1f)
            {
                lastNonZeroDirection = Mathf.Sign(controller.InputDirection.x);
            }
            
            // Handle timers
            cooldownTimer += Time.deltaTime;

            if (isDashing)
            {
                dashTimer += Time.deltaTime;
                if (dashTimer >= dashDuration)
                {
                    StopDash();
                }
            }

            // Check for dash input
            if (Input.GetKeyDown(dashKey) && !isDashing && cooldownTimer >= dashCooldown)
            {
                StartDash();
            }
        }

        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            if (isDashing)
            {
                return dashDirection * dashSpeed;
            }
            
            return velocity;
        }
        
        // --- State Control Methods ---
        private void StartDash()
        {
            isDashing = true;
            dashTimer = 0f;
            cooldownTimer = 0f;
            dashDirection = new Vector2(lastNonZeroDirection, 0);
            
            if (lastNonZeroDirection > 0.1f)
            {
                scriptAnimator.SetRightRolling(true);
            }
            else
            {
                scriptAnimator.SetLeftRolling(true);
            }

            if (dashParticles != null)
            {
                float angle = dashDirection.x > 0 ? 0 : 180;
                dashParticles.transform.rotation = Quaternion.Euler(0, angle, 0);
                dashParticles.Play();
            }
            
            controller.OnLanded(); 
        }

        private void StopDash()
        {
            isDashing = false;
            
            if (dashParticles != null)
                dashParticles.Stop();
            
            scriptAnimator.SetRightRolling(false);
            scriptAnimator.SetLeftRolling(false);
            
            Rigidbody2D rb = controller.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(rb.velocity.x * endSpeedMultiplier, 0);
        }
    }
}