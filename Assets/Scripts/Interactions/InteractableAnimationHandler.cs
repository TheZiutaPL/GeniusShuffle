using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableAnimationHandler : MonoBehaviour
{
    private enum AnimationHandlerType
    {
        DirectPlay,
        Boolean,
        Trigger,
    }

    [SerializeField] private Animator animator;

    [SerializeField] private AnimationHandlerType animationHandlerType;
    
    [SerializeField] private string animatorKeyOn;
    [SerializeField] private string animatorKeyOff;

    public void HandleAnimation(bool toggle)
    {
        switch (animationHandlerType)
        {
            case AnimationHandlerType.DirectPlay:
                animator.Play(toggle ? animatorKeyOn : animatorKeyOff);
                break;

            case AnimationHandlerType.Boolean:
                animator.SetBool(toggle ? animatorKeyOn : animatorKeyOff, toggle);
                break;

            case AnimationHandlerType.Trigger:
                animator.SetTrigger(toggle ? animatorKeyOn : animatorKeyOff);
                animator.ResetTrigger(toggle ? animatorKeyOff : animatorKeyOn);
                break;
        }
    }
}