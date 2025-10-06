using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class AntiGravityApex : MovementFeature
    {
        [Header("Apex Settings")]
        [Tooltip("Velocity threshold to be considered at apex")]
        [SerializeField] private float apexThreshold = 2f;
    
        [Tooltip("Gravity multiplier at apex")]
        [SerializeField] private float apexGravityMultiplier = 0.5f;
    
        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            if (velocity.y > 0 && velocity.y < apexThreshold)
            {
                // Reduce gravity effect at apex
                MovementConfig config = controller.GetConfig();
                float apexGravity = config.jumpGravity * apexGravityMultiplier;
                float normalGravity = config.jumpGravity;
                
                velocity.y += (apexGravity - normalGravity) * Time.fixedDeltaTime;
            }
        
            return velocity;
        }
    }
}