using UnityEngine;

namespace Script
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [SerializeField] private Animator spriteAnimator;
        [SerializeField] private Animator jetpackAnimator;

        public void TriggerUseAbility1()
        {
            spriteAnimator.SetTrigger("UseAbility1");
        }

        public void TriggerJumpAnimation()
        {
            spriteAnimator.SetTrigger("Jump");
        }

        public void SetLeftRolling(bool leftRolling)
        {
            spriteAnimator.SetBool("LeftRolling", leftRolling);
        }

        public void SetRightRolling(bool rightRolling)
        {
            spriteAnimator.SetBool("RightRolling", rightRolling);
        }

        public void SetJetActive(bool jetActive)
        {
            jetpackAnimator.SetBool("JetActive", jetActive);
        }
    }
}
