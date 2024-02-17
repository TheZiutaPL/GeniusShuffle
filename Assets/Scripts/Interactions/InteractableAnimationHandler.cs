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

    private bool passedBoolean;

    public void HandleAnimation()
    {
        passedBoolean = !passedBoolean;

        switch (animationHandlerType)
        {
            case AnimationHandlerType.DirectPlay:
                animator.Play(passedBoolean ? animatorKeyOn : animatorKeyOff);
                break;

            case AnimationHandlerType.Boolean:
                animator.SetBool(passedBoolean ? animatorKeyOn : animatorKeyOff, passedBoolean);
                break;

            case AnimationHandlerType.Trigger:
                animator.SetTrigger(passedBoolean ? animatorKeyOn : animatorKeyOff);
                animator.ResetTrigger(passedBoolean ? animatorKeyOff : animatorKeyOn);
                break;
        }
    }

    public void HandleAnimation(bool toggle)
    {
        passedBoolean = toggle;

        switch (animationHandlerType)
        {
            case AnimationHandlerType.DirectPlay:
                animator.Play(passedBoolean ? animatorKeyOn : animatorKeyOff);
                break;

            case AnimationHandlerType.Boolean:
                animator.SetBool(passedBoolean ? animatorKeyOn : animatorKeyOff, passedBoolean);
                break;

            case AnimationHandlerType.Trigger:
                animator.SetTrigger(passedBoolean ? animatorKeyOn : animatorKeyOff);
                animator.ResetTrigger(passedBoolean ? animatorKeyOff : animatorKeyOn);
                break;
        }
    }
}