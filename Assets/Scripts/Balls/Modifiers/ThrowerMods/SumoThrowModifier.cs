using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SumoThrowModifier : ThrowBase
{
    public float stompInterval = 1.4f;
    private float stompTimer = 1.4f;
    private bool left = true;
    public float stompForce;
    public float stompForceVariance;
    public float stompAngleVariance;
    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        stompTimer = stompInterval;
    }

    public override void OnThrowerUpdate(Thrower thrower)
    {
        if (!thrower.doneThrowing) return;
        base.OnThrowerUpdate(thrower);
        stompTimer -= Time.deltaTime;
        if (stompTimer < 0f) {
            stompTimer = stompInterval;
            Stomp();
            left = !left;
        }
    }

    /*public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);
        stompTimer -= Time.deltaTime;
        if (stompTimer < 0f) {
            stompTimer = stompInterval;
            Stomp();
            left = !left;
        }
    }*/
    public void Stomp() {
        CameraController.Instance.Shake(1.5f, 0.3f, 5f);
        Vector2 dir = left ? Vector2.left : Vector2.right;
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.SumoStomp);

        foreach (BallController ball in CourseController.Instance.currentBalls) {
            dir = Vector2Extension.Rotate(dir, Random.Range(0, stompAngleVariance));
            float force = (stompForce + Random.Range(-stompForceVariance, stompForceVariance));
            ball.BallJump(0.5f, 0.23f, false);
            ball.rb.AddForce(dir*force, ForceMode2D.Impulse);
        }
    }
}
