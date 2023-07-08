using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class ThrowBase : BallModifier
{
    [Header("Base Throw Values")]
    public float throwForce;
    public Vector2 throwDirection;
    public float throwForceRandomDelta = 0;
    public float throwAngleRandomDelta = 20;
    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);

        controller.rb.AddForce(Vector2Extension.Rotate((throwDirection * (throwForce + Random.Range(-throwForceRandomDelta, throwForceRandomDelta))), 
            Random.Range(-throwAngleRandomDelta, throwAngleRandomDelta)), ForceMode2D.Impulse); 
    }

}
