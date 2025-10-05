using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator spriteAnimator;

    public void PlayUseAbilityAnimation()
    {
        spriteAnimator.SetTrigger("UseAbility1");
    }
}
