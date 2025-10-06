using UnityEngine;

namespace Script
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator spriteAnimator;

        public void PlayUseAbilityAnimation()
        {
            spriteAnimator.SetTrigger("UseAbility1");
        }
    }
}
