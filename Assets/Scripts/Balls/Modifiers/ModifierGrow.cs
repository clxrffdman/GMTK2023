using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierGrow : BallModifier
{
    [Header("Extra Throw Values")]
    public float scalePerSecond;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.transform.localScale += Vector3.one * scalePerSecond * Time.deltaTime;
    }


}
