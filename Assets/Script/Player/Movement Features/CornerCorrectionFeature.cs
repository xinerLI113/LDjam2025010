using UnityEngine;

namespace Script.Player.Movement_Features
{
    public class CornerCorrectionFeature : MovementFeature
    {
        [Header("Corner Correction")]
        [Tooltip("How far to check for corners")]
        [SerializeField] private float checkDistance = 0.3f;
    
        [Tooltip("How much to nudge player")]
        [SerializeField] private float correctionAmount = 0.2f;
    
        [Tooltip("Layers to check")]
        [SerializeField] private LayerMask obstacleLayer;
    
        public override void OnPostUpdate()
        {
            if (controller.Velocity.y <= 0) return;
        
            Vector2 position = controller.transform.position;
            Collider2D col = controller.GetComponent<Collider2D>();
        
            bool hitLeft = Physics2D.Raycast(position, Vector2.left, checkDistance, obstacleLayer);
            bool hitRight = Physics2D.Raycast(position, Vector2.right, checkDistance, obstacleLayer);
        
            if (hitLeft && !hitRight)
                controller.transform.position += Vector3.right * correctionAmount * Time.fixedDeltaTime;
            else if (hitRight && !hitLeft)
                controller.transform.position += Vector3.left * correctionAmount * Time.fixedDeltaTime;
        }
    }
}