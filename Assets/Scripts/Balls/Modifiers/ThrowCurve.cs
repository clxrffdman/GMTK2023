using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ThrowCurve : ThrowBase
{
    [Header("Extra Throw Values")]
    public float curveForce;
    public Vector2 curveForceDirection;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.rb.AddForce(curveForce * 10 * curveForceDirection * Time.deltaTime, ForceMode2D.Force);
    }

    public override void OnBounce(BallController controller)
    {
        base.OnBounce(controller);

        controller.RemoveModifier(this);
    }


}
