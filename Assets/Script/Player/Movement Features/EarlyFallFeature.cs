using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class EarlyFallFeature : MovementFeature
    {
        [Header("Early Fall Settings")]
        [Tooltip("Multiplier for fall speed when jump released early")]
        [SerializeField] private float earlyFallMultiplier = 1.5f;
    
        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            if (controller.IsJumpReleased() && velocity.y > 0)
                velocity.y *= (1f - earlyFallMultiplier) * 0.5f;
        
            return velocity;
        }
    }
}