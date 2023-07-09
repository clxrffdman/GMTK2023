using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierGrow : BallModifier
{
    [Header("Extra Throw Values")]
    public float scalePerSecond;
    public float scaleCap;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.transform.localScale += Vector3.one * scalePerSecond * Time.deltaTime;
        controller.transform.localScale = Vector3.one * Mathf.Clamp(controller.transform.localScale.magnitude, 0, scaleCap);
    }


}
