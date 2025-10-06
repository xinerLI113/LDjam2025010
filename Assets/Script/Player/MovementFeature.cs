using UnityEngine;

namespace Script.Player
{
    public abstract class MovementFeature : MonoBehaviour, IMovementFeature
    {
        [SerializeField] protected bool isEnabled = true;
        protected PlayerController controller;
    
        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }
    
        public virtual void Initialize(PlayerController controller)
        {
            this.controller = controller;
        }
    
        public virtual void OnPreUpdate() { }
        public virtual Vector2 ModifyVelocity(Vector2 velocity) { return velocity; }
        public virtual void OnPostUpdate() { }
        public virtual void OnLanded() { }
        public virtual void OnLeftGround() { }
    }
}