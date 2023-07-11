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
    public float zigZagPauseInterval = 1.5f;
    private float zigZagPauseTimer = 0f;

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
        zigZagPauseTimer = zigZagPauseTimer > 0f ? zigZagPauseTimer - Time.deltaTime : 0f;
        counter = counter > 0 ? counter - Time.deltaTime : 0f;
        if (zigZagPauseTimer <= 0f && counter <= 0f)
        {
            controller.rb.velocity = new Vector2(0, controller.rb.velocity.y);

            zigZagDirection *= -1;
            controller.rb.AddForce(zigZagDirection * zigZagForce, ForceMode2D.Impulse);
            Debug.Log(zigZagDirection * zigZagForce);
            counter = zigZagInterval;
            if (initialThrow)
            {
                counter = zigZagInterval / 2f;
                initialThrow = false;
            }
        }
    }

    public override void OnBounce(BallController controller)
    {
        base.OnBounce(controller);
        zigZagPauseTimer = zigZagPauseInterval;
    }
}
