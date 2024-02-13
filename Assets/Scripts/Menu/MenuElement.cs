using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : InteractionHandler
{
    [SerializeField] bool playOnClickByDefault = true; // You don't want a slider to play a sound on click but on change value, so this is used for that
    [SerializeField] AudioClip[] onHover;
    [SerializeField] AudioClip[] onUnhover;
    [SerializeField] AudioClip[] onClick;

    public override void HoverAction(bool hover)
    {
        if (hover && onHover.Length > 0)
            AudioManager.PlaySound(onHover);
        else if (onUnhover.Length > 0)
            AudioManager.PlaySound(onUnhover);

        base.HoverAction(hover);
    }

    public override void ClickAction(Vector3 hitPoint)
    {
        if (playOnClickByDefault)
            PlayClickSound();

        base.ClickAction(hitPoint);
    }

    protected void PlayClickSound()
    {
        if (onClick.Length > 0)
            AudioManager.PlaySound(onClick);
    }
}
