using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class StickyFeetFeature : MovementFeature
    {
        [Header("Sticky Feet Settings")]
        [Tooltip("Friction multiplier when pressing opposite direction on landing")]
        [SerializeField] private float frictionMultiplier = 3f;
    
        [Tooltip("How long after landing does this apply")]
        [SerializeField] private float stickyDuration = 0.2f;
    
        private float landingTime;
    
        public override void OnLanded()
        {
            landingTime = Time.time;
        }
    
        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            // Check if we just landed
            float timeSinceLanding = Time.time - landingTime;
            if (controller.IsGrounded && timeSinceLanding < stickyDuration)
            {
                float input = controller.InputDirection.x;
                bool pressingOpposite = (input < 0 && velocity.x > 0) || (input > 0 && velocity.x < 0);
            
                if (pressingOpposite)
                {
                    MovementConfig config = controller.GetConfig();
                    velocity.x = Mathf.MoveTowards(
                        velocity.x,
                        0,
                        config.groundAcceleration * frictionMultiplier * Time.fixedDeltaTime
                    );
                }
            }
        
            return velocity;
        }
    }
}