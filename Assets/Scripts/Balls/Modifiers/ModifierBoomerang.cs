using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBoomerang : BallModifier
{
    public float boomerangForce = 10;
    public Vector2 boomerangDirection = new Vector2(0, 1);
    public float courseThreshhold = 0.7f;

    public float killThreshhold = 0.45f;
    public bool crossedThreshhold = false;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if(coursePercentage > courseThreshhold)
        {
            crossedThreshhold = true;
            controller.rb.AddForce(boomerangForce * boomerangDirection * Time.deltaTime, ForceMode2D.Impulse);
        }

        if(coursePercentage < killThreshhold && crossedThreshhold)
        {
            controller.StartCoroutine(controller.DeleteBall());
        }

    }
}
