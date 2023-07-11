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
            //controller.FadeOut(fadeoutDuration);
            Color spriteColor = controller.ballSprite.color;
            Color newColor = spriteColor;
            newColor.a = 0f;
            LeanTween.value(controller.ballSprite.gameObject, (Color val) => { controller.ballSprite.color = val; }, spriteColor, newColor, fadeoutDuration);
            //FadeBall(controller);
            controller.RemoveModifier(this);
        }

    }

    /*public void FadeBall(BallController controller)
    {
        
        controller.StartCoroutine(GlobalFunctions.FadeOut(controller.ballSprite, fadeoutDuration));
        //controller.StartCoroutine(GlobalFunctions.FadeOut(controller.ballShadow, fadeoutDuration));
    }*/
}
