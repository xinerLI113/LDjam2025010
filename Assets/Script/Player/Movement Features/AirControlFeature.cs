using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class AirControlFeature : MovementFeature
    {
        [Header("Air Control Settings")]
        [Tooltip("Multiplier for direction change speed in air")]
        [SerializeField] private float airControlMultiplier = 1.5f;
    
        [Tooltip("Only boost control near apex")]
        [SerializeField] private bool onlyAtApex = true;
    
        [SerializeField] private float apexThreshold = 3f;
    
        public override Vector2 ModifyVelocity(Vector2 velocity)
        {
            if (!controller.IsGrounded)
            {
                bool atApex = Mathf.Abs(velocity.y) < apexThreshold;
            
                if (!onlyAtApex || atApex)
                {
                    float input = controller.InputDirection.x;
                    bool changingDirection = (input < 0 && velocity.x > 0) || (input > 0 && velocity.x < 0);
                
                    if (changingDirection)
                    {
                        MovementConfig config = controller.GetConfig();
                        float targetSpeed = input * config.moveSpeed;
                        velocity.x = Mathf.MoveTowards(
                            velocity.x,
                            targetSpeed,
                            config.airAcceleration * airControlMultiplier * Time.fixedDeltaTime
                        );
                    }
                }
            }
        
            return velocity;
        }
    }
}