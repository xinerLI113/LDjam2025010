using UnityEngine;
using System.Collections.Generic;


namespace Script.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        public MovementConfig config;
        
        [Header("Layer Masks")]
        [Tooltip("Set this to the layer your water objects are on.")]
        public LayerMask waterLayer;
        
        // Components
        private Rigidbody2D rb;
        private Collider2D col;
        
        // States
        public Vector2 Velocity { get; private set; }
        public bool IsGrounded { get; private set; }
        public bool IsInWater { get; private set; }
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
        private PlayerAnimatorController scriptAnimator;
        
        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            col = GetComponent<Collider2D>();
            rb.freezeRotation = true;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.gravityScale = 0; // We handle gravity manually
            scriptAnimator = FindObjectOfType<PlayerAnimatorController>();
            
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

            // Let features process pre-update
            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnPreUpdate();
            }
        }
        
        void FixedUpdate()
        {
            Velocity = rb.velocity;

            TimeSinceJumpPressed += Time.fixedDeltaTime;
            TimeSinceGrounded += Time.fixedDeltaTime;

            if (IsInWater && jumpPressed && config.waterJumpImpulse > 0)
            {
                rb.AddForce(Vector2.up * config.waterJumpImpulse, ForceMode2D.Impulse);
                TimeSinceJumpPressed = config.jumpBufferTime + 0.1f;
            }
            
            WasGroundedLastFrame = IsGrounded;
            CheckGrounded(); 

            Vector2 newVelocity = CalculateMovement();

            if (!IsInWater) 
            {
                foreach (var feature in features)
                {
                    if (feature.IsEnabled)
                        newVelocity = feature.ModifyVelocity(newVelocity);
                }
            }
    
            Velocity = newVelocity;
            VerticalVelocity = newVelocity.y;
            rb.velocity = newVelocity;

            foreach (var feature in features)
            {
                if (feature.IsEnabled)
                    feature.OnPostUpdate();
            }
        }

        private Vector2 CalculateMovement()
        {
            Vector2 velocity = Velocity;

            float targetSpeed = InputDirection.x * config.moveSpeed;
            float acceleration = IsGrounded ? config.groundAcceleration : config.airAcceleration;

            velocity.x = Mathf.MoveTowards(
                velocity.x,
                targetSpeed,
                acceleration * Time.fixedDeltaTime
            );
            
            if (InputDirection.x != 0f) scriptAnimator.SetIsMoving(true);
            else scriptAnimator.SetIsMoving(false);

            if (IsInWater)
            {
                velocity.y = rb.velocity.y;
            }
            else
            {
                if (CanJump())
                {
                    StartJump();
                }

                if (IsJumping)
                {
                    if (jumpHeld && JumpTimeCounter < config.jumpTime)
                    {
                        velocity.y = config.jumpForce;
                        JumpTimeCounter += Time.fixedDeltaTime;
                    }
                    else
                    {
                        IsJumping = false;
                    }
                }

                if (!IsJumping)
                {
                    float gravity = velocity.y > 0 ? config.jumpGravity : config.fallGravity;
                    velocity.y += gravity * Time.fixedDeltaTime;
                    velocity.y = Mathf.Max(velocity.y, -config.maxFallSpeed);
                }
            }

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
            // Water Jump Logic
            if (IsInWater)
            {
                if (config.canJumpInWater)
                {
                    return jumpPressed && !IsJumping;
                }
                else
                {
                    return false;
                }
            }

            // Ground/Air Jump Logic (if not in water)
            bool groundJump = IsGrounded && jumpPressed;

            // Jump buffering
            bool bufferedJump = IsGrounded && TimeSinceJumpPressed < config.jumpBufferTime;

            // Coyote time
            bool coyoteJump = !IsGrounded && TimeSinceGrounded < config.coyoteTime && jumpPressed;

            return (groundJump || bufferedJump || coyoteJump) && !IsJumping;
        }
        
        private void StartJump()
        {
            IsJumping = true;
            JumpTimeCounter = 0f;
            TimeSinceGrounded = config.coyoteTime + 0.01f; // Prevent double coyote jump
            TimeSinceJumpPressed = config.jumpBufferTime + 0.01f; // Prevent double buffered jump
            scriptAnimator.TriggerJumpAnimation();
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
        
        void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the layer of the object we entered is in our waterLayer mask
            if ((waterLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                EnterWater();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            // Check if the layer of the object we exited is in our waterLayer mask
            if ((waterLayer.value & (1 << other.gameObject.layer)) > 0)
            {
                ExitWater();
            }
        }

        private void EnterWater()
        {
            if (IsInWater) return;

            IsInWater = true;
            IsJumping = false;
            rb.gravityScale = 1;
            
            scriptAnimator.SetIsInWater(true);
        }

        private void ExitWater()
        {
            if (!IsInWater) return;

            IsInWater = false;
            rb.gravityScale = 0;
            
            scriptAnimator.SetIsInWater(false);
        }
        
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