using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public abstract class BallModifier : ScriptableObject
{
    public string description;
    public bool hasColorOverride;
    [ShowIf("hasColorOverride")]
    [ColorUsage(true, true)]
    public Color outlineColor;

    public virtual void OnSpawn(BallController controller)
    {
        SetOutlineColor(controller.ballSprite);
    }
    public bool SetOutlineColor(SpriteRenderer sprite) {
        if (hasColorOverride)
        {
            sprite.material.SetColor("_GlowColor", outlineColor);
            return true;
        }
        return false;
    }

    public virtual void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {

    }

    public virtual void OnBounce(BallController controller)
    {

    }
}
