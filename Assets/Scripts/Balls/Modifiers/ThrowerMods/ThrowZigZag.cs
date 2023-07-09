using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowZigZag : ThrowBase
{
    [Header("Extra Throw Values")]
    public float zigZagForce = 500;
    public Vector2 zigZagDirection = new Vector2(1, 0f);
    public bool doZigZag;
    private float counter = 0;
    public float zigZagInterval = 1f;
    private bool initialThrow = true;

    [Header("Variance")]
    public float intervalOffset = 0;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        counter = zigZagInterval;
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);
        if (counter >= 0)
        {
            counter -= Time.deltaTime;
        }     
        else
        {
            controller.rb.velocity = new Vector2(0, controller.rb.velocity.y);
            if (initialThrow)
            {
                initialThrow = false;
                zigZagForce *= 2;
            }

            zigZagDirection *= -1;
            controller.rb.AddForce(zigZagDirection * zigZagForce * Time.deltaTime, ForceMode2D.Impulse);
            Debug.Log(zigZagDirection * zigZagForce);
            counter = zigZagInterval;
        }
    }

    public override void OnBounce(BallController controller)
    {
        base.OnBounce(controller);

        controller.RemoveModifier(this);
    }
}
