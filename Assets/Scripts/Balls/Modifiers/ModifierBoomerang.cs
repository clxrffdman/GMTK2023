using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierBoomerang : BallModifier
{
    public float boomerangForce = 10;
    public float constantUpForce = 40f;
    public Vector2 boomerangDirection = new Vector2(0, 1);
    public float courseThreshhold = 0.7f;
    public float trackingForce = 10f;
    public float trackingDist = 0.3f;

    public float killThreshhold = 0.45f;
    public bool crossedThreshhold = false;
    private bool deleted=false;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if(coursePercentage > courseThreshhold)
        {
            crossedThreshhold = true;
            controller.rb.AddForce(boomerangForce * boomerangDirection * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (crossedThreshhold) {
            controller.rb.AddForce(constantUpForce * boomerangDirection * Time.deltaTime);
        }
        
        if (Mathf.Abs(PlayerController.Instance.transform.position.x-controller.transform.position.x) > trackingDist) {
            Vector2 trackDir = PlayerController.Instance.transform.position.x < controller.transform.position.x ? new Vector2(-1f,0f):new Vector2(1f,0f);
            controller.rb.AddForce(trackDir * trackingForce * Time.deltaTime);
        }
    
        if(coursePercentage < killThreshhold && crossedThreshhold && !deleted)
        {
            deleted = true;
            controller.StartCoroutine(controller.DeleteBall());
        }

    }
}
