using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSplit : BallModifier
{
    [Header("Extra Throw Values")]
    public float splitPercentage = 0.5f;
    public int splitCount = 2;
    public float projectileSpread = 50;
    public float spreadForce = 30;

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if (coursePercentage > splitPercentage)
        {
            SplitBall(controller);
        }
    }

    public void SplitBall(BallController controller)
    {
        float angleBetweenProjectiles = projectileSpread / (splitCount - 1);

        for (int i = 0; i < splitCount; i++)
        {
            float spread = -(projectileSpread / 2) + (i * angleBetweenProjectiles);
            SpawnNewBall(controller, spread);
        }
        controller.RemoveModifier(this);
        CourseController.Instance.currentBalls.Remove(controller);
        Destroy(controller.gameObject);
    }

    public void SpawnNewBall(BallController controller, float spread)
    {
        Vector2 dir = Vector2Extension.Rotate(controller.GetComponent<Rigidbody2D>().velocity.normalized, spread);
        var bullet = Instantiate(controller.transform.gameObject, controller.transform.position, Quaternion.identity);
        BallController ballCont = GlobalFunctions.FindComponent<BallController>(bullet.gameObject);
        ballCont.RemoveModifier(this);
        ballCont.rb.velocity = Vector2.zero;
        ballCont.rb.AddForce(dir * spreadForce, ForceMode2D.Impulse);
        ballCont.SetIgnoreBallDuration(0.2f);
        CourseController.Instance.currentBalls.Add(ballCont);

    }
}
