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
    public bool applyToThrower = false;

    public bool useCustomSprite;
    [ShowIf("useCustomSprite")]
    public Sprite customSprite;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        SetCustomSprite(controller.ballSprite);

        controller.rb.AddForce(Vector2Extension.Rotate((throwDirection * (throwForce + Random.Range(-throwForceRandomDelta, throwForceRandomDelta))), 
            Random.Range(-throwAngleRandomDelta, throwAngleRandomDelta)), ForceMode2D.Impulse); 
    }
    public bool SetCustomSprite(SpriteRenderer sprite) {
        if (useCustomSprite) {
            sprite.sprite = customSprite;
            return true;
        }
        return false;
    }
    public virtual void OnThrowerSpawn(Thrower thrower) {
        return;
    }
    public virtual void OnThrowerUpdate(Thrower thrower) {
        return;
    }

}
