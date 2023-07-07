using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBallGravity : BallModifier
{
    public float gravity;
    public Vector2 direction;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.rb.AddForce(direction * gravity * Time.deltaTime);
    }


}
