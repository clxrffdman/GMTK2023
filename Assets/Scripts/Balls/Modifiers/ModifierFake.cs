using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierFake : BallModifier
{
    public float courseActivationPercentage = 0.8f;
    public float fadeoutDuration = 0.4f;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if(coursePercentage >= courseActivationPercentage)
        {
            FadeBall(controller);
            controller.RemoveModifier(this);
        }

    }

    public void FadeBall(BallController controller)
    {
        controller.StartCoroutine(GlobalFunctions.FadeOut(controller.ballSprite, fadeoutDuration));
        controller.StartCoroutine(GlobalFunctions.FadeOut(controller.ballShadow, fadeoutDuration));
    }
}
