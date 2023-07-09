using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaThrowModifier : ThrowBase
{
    public BallModifier fakeBallModifier;
    public int splitCount = 2;
    public float projectileSpread = 50;
    public float spreadForce = 20;
    public LayerMask ignoreCollLayermask;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        SpawnFakeBalls(controller);
    }

    public void SpawnFakeBalls(BallController controller)
    {
        float angleBetweenProjectiles = projectileSpread / (splitCount - 1);

        for (int i = 0; i < splitCount; i++)
        {
            float spread = -(projectileSpread / 2) + (i * angleBetweenProjectiles);
            SpawnFakeBall(controller, spread);
        }
        controller.RemoveModifier(this);
    }

    public void SpawnFakeBall(BallController controller, float spread)
    {
        Vector2 dir = Vector2Extension.Rotate(controller.GetComponent<Rigidbody2D>().velocity.normalized, spread);
        var bullet = Instantiate(controller.transform.gameObject, controller.transform.position, Quaternion.identity);
        bullet.transform.GetComponent<BallController>().AddModifier(fakeBallModifier);
        bullet.transform.GetComponent<BallController>().defaultLayerMask = ignoreCollLayermask;
        bullet.transform.GetComponent<BallController>().rb.velocity = Vector2.zero;
        bullet.transform.GetComponent<BallController>().rb.AddForce(dir * spreadForce, ForceMode2D.Impulse);
        bullet.transform.GetComponent<BallController>().SetIgnoreBallDuration(0.2f);
        bullet.transform.GetComponent<BallController>().RemoveModifier(this);
    }



}
