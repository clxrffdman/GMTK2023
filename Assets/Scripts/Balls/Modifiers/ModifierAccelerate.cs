using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierAccelerate : BallModifier
{
    [Header("Extra Throw Values")]
    public float accelForce;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.rb.AddForce(controller.rb.velocity.normalized * accelForce * 10 * Time.deltaTime, ForceMode2D.Impulse);
    }
}
