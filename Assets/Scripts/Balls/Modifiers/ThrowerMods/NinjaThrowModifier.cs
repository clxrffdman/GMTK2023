using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaThrowModifier : ThrowBase
{
    public BallModifier fakeBallModifier;
    public RuntimeAnimatorController fakeAnim;
    public Color fakeColor;
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
        float angleBetweenProjectiles = projectileSpread / (splitCount);
        int randomReal = Random.Range(0, splitCount + 1);
        Vector2 baseDir = controller.GetComponent<Rigidbody2D>().velocity.normalized;
        for (int i = 0; i < splitCount+1; i++)
        {
            float spread = -(projectileSpread / 2) + (i * angleBetweenProjectiles);
            if (i == randomReal)
            {
                RealignExistingBall(controller, spread, baseDir);
            }
            else
            {
                SpawnFakeBall(controller, spread, baseDir);
            }
            
            
        }
        controller.RemoveModifier(this);
    }
    
    public void RealignExistingBall(BallController controller, float spread, Vector2 baseDir)
    {
        Vector2 dir = Vector2Extension.Rotate(baseDir, spread);
        controller.rb.velocity = dir * controller.rb.velocity.magnitude;
        controller.SetIgnoreBallDuration(0.2f);
        controller.RemoveModifier(this);
    }

    public void SpawnFakeBall(BallController controller, float spread, Vector2 baseDir)
    {
        Vector2 dir = Vector2Extension.Rotate(baseDir, spread);
        var bullet = Instantiate(controller.transform.gameObject, controller.transform.position, Quaternion.identity);
        bullet.gameObject.GetComponentInChildren<SpriteRenderer>().transform.localPosition = new Vector2(0f, 0f);
        bullet.gameObject.GetComponentInChildren<SpriteRenderer>().color = fakeColor;//new Color(220f,220f,220f,1f);
        //bullet.gameObject.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        bullet.transform.Find("Shadow").gameObject.SetActive(false);
        //bullet.gameObject.GetComponentInChildren<Animator>().runtimeAnimatorController = fakeAnim;
        //bullet.gameObject.GetComponentInChildren<Animator>().SetBool("thrown", true);
        bullet.transform.GetComponent<BallController>().AddModifier(fakeBallModifier);
        bullet.transform.GetComponent<BallController>().defaultLayerMask = ignoreCollLayermask;
        bullet.transform.GetComponent<BallController>().rb.velocity = dir * controller.rb.velocity.magnitude;
        bullet.transform.GetComponent<BallController>().SetIgnoreBallDuration(0.2f);
        bullet.transform.GetComponent<BallController>().RemoveModifier(this);
    }
}
