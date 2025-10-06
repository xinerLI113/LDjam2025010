using UnityEngine;

namespace Script.Player
{
    [CreateAssetMenu(fileName = "MovementConfig", menuName = "Player/Movement Config")]
    public class MovementConfig : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed = 7f;
        public float groundAcceleration = 70f;
        public float airAcceleration = 40f;
    
        [Header("Jumping")]
        public float jumpForce = 12f;
        public float jumpTime = 0.35f;
        public float jumpGravity = -25f;
        public float fallGravity = -40f;
        public float maxFallSpeed = 20f;
    
        [Header("Jump Assistance")]
        [Tooltip("Time after pressing jump that it still registers on landing")]
        public float jumpBufferTime = 0.15f;
    
        [Tooltip("Time after leaving ground where jump still works")]
        public float coyoteTime = 0.1f;
    
        [Header("Ground Detection")]
        public LayerMask groundLayer;
        public float groundCheckDistance = 0.1f;
        public float groundCheckWidth = 0.9f;
        
        [Header("Water Physics")]
        [Tooltip("How quickly the player's vertical speed slows to zero in water. Higher is faster.")]
        public float waterVerticalDrag = 15f;
    }
}