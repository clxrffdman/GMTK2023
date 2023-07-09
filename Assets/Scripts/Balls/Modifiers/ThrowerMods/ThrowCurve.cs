using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ThrowCurve : ThrowBase
{
    [Header("Extra Throw Values")]
    public float curveForce = 50;
    private float extraCurveForce = 0;
    public Vector2 curveForceDirection = new Vector2(1, -0.2f);

    [Header("Variance")]
    public float randomExtraCurveForce = 0;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        extraCurveForce = Random.Range(0, randomExtraCurveForce);
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.rb.AddForce((curveForce + extraCurveForce) * 10 * curveForceDirection * Time.deltaTime, ForceMode2D.Force);
    }

    public override void OnBounce(BallController controller)
    {
        base.OnBounce(controller);

        controller.RemoveModifier(this);
    }


}
