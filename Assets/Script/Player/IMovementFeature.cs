using UnityEngine;

namespace Script.Player
{
    public interface IMovementFeature
    {
        bool IsEnabled { get; set; }
        void Initialize(PlayerController controller);
        void OnPreUpdate();
        Vector2 ModifyVelocity(Vector2 velocity);
        void OnPostUpdate();
        void OnLanded();
        void OnLeftGround();
    }
}