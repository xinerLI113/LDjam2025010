using UnityEngine;

namespace Script.Player.Movement_Features
{
    using UnityEngine;

    public class VariableJumpHeightFeature : MovementFeature
    {
        [Header("Variable Jump Settings")]
        [Tooltip("Minimum jump force (short tap)")]
        [SerializeField] private float minJumpForce = 8f;
    
        [Tooltip("Maximum jump force (full hold)")]
        [SerializeField] private float maxJumpForce = 15f;
    
        [Tooltip("Speed affects jump height")]
        [SerializeField] private bool speedAffectsHeight = false;
    
        [SerializeField] private float speedMultiplier = 0.3f;
    
        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            if (controller.IsJumping && controller.IsJumpHeld())
            {
                float jumpProgress = controller.JumpTimeCounter / controller.GetConfig().jumpTime;
                float jumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, jumpProgress);
            
                if (speedAffectsHeight)
                {
                    float speedBonus = Mathf.Abs(velocity.x) * speedMultiplier;
                    jumpForce += speedBonus;
                }
            
                if (velocity.y > 0)
                    velocity.y = jumpForce;
            }
        
            return velocity;
        }
    }
}