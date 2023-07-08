using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBoomerang : BallModifier
{
    public float boomerangForce = 10;
    public Vector2 boomerangDirection = new Vector2(0, 1);
    public float timeThreshhold = 1;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if(activeTime > timeThreshhold)
        {
            controller.rb.AddForce(boomerangForce * boomerangDirection * Time.deltaTime, ForceMode2D.Impulse);
        }

    }
}
