using UnityEngine;
using System.Collections.Generic;


namespace Script.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public MovementConfig config;
        
        // Components
        private Rigidbody2D rb;
        private Collider2D col;
        
        // States
        public Vector2 Velocity { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool WasGroundedLastFrame { get; private set; }
        public float TimeSinceGrounded { get; private set; }
        public float TimeSinceJumpPressed { get; private set; }
        public bool IsJumping { get; private set; }
        public float JumpTimeCounter { get; private set; }
        public Vector2 InputDirection { get; private set; }
        public float VerticalVelocity { get; private set; }
        
        // Movement features
        private List<IMovementFeature> features = new List<IMovementFeature>();
        
        // Internal
        private bool jumpPressed;
        private bool jumpHeld;
        private bool jumpReleased;
        private float apexPoint;
        
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = 0; // We handle gravity manually
            
            // Gather all movement features
            features.AddRange(GetComponents<IMovementFeature>());
        }
        
        void Start()
        {
            foreach (var feature in features)
            {
                feature.Initialize(this);
            }
        }
        
        void Update()
        {
            // Gather input
            GatherInput();
    
            // UpdateTimers(); // << REMOVE THIS LINE

            // Let features process pre-update
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnPreUpdate();
            }
        }
        
        void FixedUpdate()
        {
            // Sync from Rigidbody
            Velocity = rb.velocity;

            // --- NEW: UPDATE TIMERS HERE ---
            // Update all timers in FixedUpdate to be in sync with physics
            TimeSinceJumpPressed += Time.fixedDeltaTime;
            TimeSinceGrounded += Time.fixedDeltaTime;
            // --- END NEW SECTION ---

            // Check ground state
            WasGroundedLastFrame = IsGrounded;
            CheckGrounded(); // This will reset TimeSinceGrounded if we're on the ground

            // Calculate new velocities
            Vector2 newVelocity = CalculateMovement();

            // Features modify velocity
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    newVelocity = feature.ModifyVelocity(newVelocity);
            }
    
            Velocity = newVelocity;
            VerticalVelocity = newVelocity.y;

            // Apply velocity ONCE at the very end
            rb.velocity = newVelocity;

            // Features process post-update
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnPostUpdate();
            }
        }
        
        private Vector2 CalculateMovement()
        {
            Vector2 velocity = Velocity;
    
            // Horizontal movement
            float targetSpeed = InputDirection.x * config.moveSpeed;
            float acceleration = IsGrounded ? config.groundAcceleration : config.airAcceleration;
    
            velocity.x = Mathf.MoveTowards(
                velocity.x,
                targetSpeed,
                acceleration * Time.fixedDeltaTime
            );
    
            // Vertical movement
            float verticalVelocity = velocity.y;
    
            // Handle jumping
            if (CanJump())
            {
                StartJump();
            }
    
            if (IsJumping)
            {
                if (jumpHeld && JumpTimeCounter < config.jumpTime)
                {
                    verticalVelocity = config.jumpForce;
                    JumpTimeCounter += Time.fixedDeltaTime;
                }
                else
                {
                    IsJumping = false;
                }
            }
    
            // Apply gravity
            if (!IsJumping || !jumpHeld)
            {
                float gravity = verticalVelocity > 0 ? config.jumpGravity : config.fallGravity;
                verticalVelocity += gravity * Time.fixedDeltaTime;
                verticalVelocity = Mathf.Max(verticalVelocity, -config.maxFallSpeed);
            }
    
            velocity.y = verticalVelocity;
    
            return velocity;
        }
        
        private void GatherInput()
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            InputDirection = new Vector2(horizontal, vertical);
    
            jumpPressed = Input.GetButtonDown("Jump");
            jumpHeld = Input.GetButton("Jump");
            jumpReleased = Input.GetButtonUp("Jump");
    
            if (jumpPressed)
            {
                // Reset timer on the frame the button is pressed
                TimeSinceJumpPressed = 0f;
            }
        }
        
        private void CheckGrounded()
        {
            // Raycast down
            Vector2 origin = (Vector2)transform.position + col.offset;
            RaycastHit2D hit = Physics2D.BoxCast(
                origin,
                new Vector2(col.bounds.size.x * config.groundCheckWidth, 0.1f),
                0f,
                Vector2.down,
                config.groundCheckDistance,
                config.groundLayer
            );
    
            IsGrounded = hit.collider != null;
    
            if (IsGrounded)
            {
                // Reset timer on the frame we are grounded
                TimeSinceGrounded = 0f;
            }

            // Just landed
            if (IsGrounded && !WasGroundedLastFrame)
            {
                OnLanded();
            }
    
            // Just left ground
            if (!IsGrounded && WasGroundedLastFrame)
            {
                OnLeftGround();
            }
        }
        
        private bool CanJump()
        {
            // Standard jump check (immediate ground jump)
            bool groundJump = IsGrounded && jumpPressed;

            // Jump buffering - allow jump if pressed recently while grounded
            bool bufferedJump = IsGrounded && TimeSinceJumpPressed < config.jumpBufferTime;

            // Coyote time - allow jump if:
            // - Not currently grounded
            // - Recently left ground (within coyote time)
            // - Jump was pressed recently (within buffer time)
            bool coyoteJump = !IsGrounded 
                              && TimeSinceGrounded < config.coyoteTime 
                              && TimeSinceJumpPressed < config.jumpBufferTime;

            return (groundJump || bufferedJump || coyoteJump) && !IsJumping;
        }
        
        private void StartJump()
        {
            IsJumping = true;
            JumpTimeCounter = 0f;
            TimeSinceGrounded = config.coyoteTime + 0.01f; // Prevent double coyote jump
            TimeSinceJumpPressed = config.jumpBufferTime + 0.01f; // Prevent double buffered jump
        }
        
        public void OnLanded()
        {
            IsJumping = false;
            JumpTimeCounter = 0f;
            
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnLanded();
            }
        }
        
        public void OnLeftGround()
        {
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnLeftGround();
            }
        }
        
        // Public methods for features
        public MovementConfig GetConfig() => config;
        public bool IsJumpPressed() => jumpPressed;
        public bool IsJumpHeld() => jumpHeld;
        public bool IsJumpReleased() => jumpReleased;
        public float GetApexPoint() => apexPoint;
        
        void OnDrawGizmos()
        {
            if (col == null)
            {
                col = GetComponent<Collider2D>();
                if (col == null) return;
            }
            
            if (config == null) return;
            
            // Calculate ground check parameters
            Vector2 origin = (Vector2)transform.position + col.offset;
            Vector2 boxSize = new Vector2(col.bounds.size.x * config.groundCheckWidth, 0.1f);
            
            // Draw the starting box
            Gizmos.color = Color.yellow;
            DrawBox(origin, boxSize);
            
            // Draw the cast direction
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin, origin + Vector2.down * config.groundCheckDistance);
            
            // Draw the end box
            Vector2 endPosition = origin + Vector2.down * config.groundCheckDistance;
            Gizmos.color = IsGrounded ? Color.green : Color.red;
            DrawBox(endPosition, boxSize);
            
            // Perform the actual check to see what we hit
            RaycastHit2D hit = Physics2D.BoxCast(
                origin,
                boxSize,
                0f,
                Vector2.down,
                config.groundCheckDistance,
                config.groundLayer
            );
            
            // If we hit something, draw the hit point
            if (hit.collider != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireSphere(hit.point, 0.1f);
                
                // Draw hit info text in Scene view
                #if UNITY_EDITOR
                UnityEditor.Handles.Label(
                    hit.point + Vector2.up * 0.3f, 
                    $"Hit: {hit.collider.gameObject.name}\nDistance: {hit.distance:F3}"
                );
                #endif
            }
            
            // Draw additional info
            #if UNITY_EDITOR
            UnityEditor.Handles.Label(
                transform.position + Vector3.up * 1.5f,
                $"Grounded: {IsGrounded}\nVelocity: {Velocity}\nTimeSinceGrounded: {TimeSinceGrounded:F2}"
            );
            #endif
        }

        // Helper method to draw a box
        private void DrawBox(Vector2 center, Vector2 size)
        {
            Vector2 halfSize = size / 2f;
            Vector2 topLeft = center + new Vector2(-halfSize.x, halfSize.y);
            Vector2 topRight = center + new Vector2(halfSize.x, halfSize.y);
            Vector2 bottomLeft = center + new Vector2(-halfSize.x, -halfSize.y);
            Vector2 bottomRight = center + new Vector2(halfSize.x, -halfSize.y);
            
            Gizmos.DrawLine(topLeft, topRight);
            Gizmos.DrawLine(topRight, bottomRight);
            Gizmos.DrawLine(bottomRight, bottomLeft);
            Gizmos.DrawLine(bottomLeft, topLeft);
        }
        
        void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 16;
            style.normal.textColor = Color.white;
    
            GUI.Label(new Rect(10, 10, 400, 25), $"Grounded: {IsGrounded}", style);
            GUI.Label(new Rect(10, 35, 400, 25), $"Input: {InputDirection.x:F2}", style);
            GUI.Label(new Rect(10, 60, 400, 25), $"Velocity: {Velocity}", style);
            GUI.Label(new Rect(10, 85, 400, 25), $"RB Velocity: {rb.velocity}", style);
            GUI.Label(new Rect(10, 110, 400, 25), $"RB Drag: {rb.drag}", style);
    
            // Check for physics material
            PhysicsMaterial2D mat = col.sharedMaterial;
            string matInfo = mat == null ? "None" : $"Friction: {mat.friction}";
            GUI.Label(new Rect(10, 135, 400, 25), $"Physics Mat: {matInfo}", style);
        }
    }
}