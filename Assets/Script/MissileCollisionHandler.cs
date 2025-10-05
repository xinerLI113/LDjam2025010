using UnityEngine;

namespace Script
{
    public class MissileCollisionHandler : MonoBehaviour
    {
        private TopdownMissileBehaviour parentBehaviour;
    
        public void Initialize(TopdownMissileBehaviour parent)
        {
            parentBehaviour = parent;
        }
    
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Water")) return;
            if (parentBehaviour != null)
                parentBehaviour.OnMissileImpact(transform.position);
            
            Destroy(gameObject);
        }
    }
}