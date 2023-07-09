using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSlow : BallModifier
{
    public float slowAmount = 1;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        controller.rb.velocity = controller.rb.velocity - (controller.rb.velocity * coursePercentage * Time.deltaTime * slowAmount);
    }
}
