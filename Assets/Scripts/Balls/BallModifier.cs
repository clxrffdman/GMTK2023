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
        if (hasColorOverride)
        {
            controller.ballSprite.material.SetColor("_GlowColor", outlineColor);
        }
    }

    public virtual void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {

    }

    public virtual void OnBounce(BallController controller)
    {

    }
}
