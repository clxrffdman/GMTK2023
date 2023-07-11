using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBallGravity : BallModifier
{
    public float gravity;
    public float upGravity;
    public Vector2 direction;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);
        if (controller.rb.velocity.y >= 0) {
            controller.rb.AddForce(direction * upGravity * Time.deltaTime);
            return;
        }
        controller.rb.AddForce(direction * gravity * Time.deltaTime);
    }


}
