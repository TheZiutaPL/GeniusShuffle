using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuElement : InteractionHandler
{
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

    protected void PlayClickSound()
    {
        if (onClick.Length > 0)
            AudioManager.PlaySound(onClick);
    }
}
