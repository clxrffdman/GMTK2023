using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierSplit : BallModifier
{
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
        Destroy(controller.gameObject);
    }

    public void SpawnNewBall(BallController controller, float spread)
    {
        Vector2 dir = Vector2Extension.Rotate(controller.GetComponent<Rigidbody2D>().velocity.normalized, spread);
        var bullet = Instantiate(controller.transform.gameObject, controller.transform.position, Quaternion.identity);
        bullet.transform.GetComponent<BallController>().RemoveModifier(this);
        bullet.transform.GetComponent<BallController>().rb.velocity = Vector2.zero;
        bullet.transform.GetComponent<BallController>().rb.AddForce(dir * spreadForce, ForceMode2D.Impulse);
        bullet.transform.GetComponent<BallController>().SetIgnoreBallDuration(0.2f);

    }
}
