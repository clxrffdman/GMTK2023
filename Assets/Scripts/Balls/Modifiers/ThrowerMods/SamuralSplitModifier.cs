using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuralSplitModifier : ThrowBase
{
    public float splitTime;
    public float currentSplitTimer = 0f;
    public int splitCount = 2;
    public float projectileSpread = 50;
    public float spreadForce = 30;
    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        currentSplitTimer = splitTime;
    }

    public override void OnThrowerUpdate(Thrower thrower)
    {
        if (!thrower.doneThrowing) return;
        base.OnThrowerUpdate(thrower);
        currentSplitTimer -= Time.deltaTime;
        if (currentSplitTimer < 0f)
        {
            thrower.StartCoroutine(SamuraiSplit(thrower));
            thrower.currThrowMods.Remove(this);
        }
    }

    public IEnumerator SamuraiSplit(Thrower thrower)
    {
        GameplayUIManager.Instance.transitionPanelController.BeginTransition(0.35f, 2.5f, .3f);
        yield return new WaitForSeconds(2.9f);

        GameplayUIManager.Instance.transitionPanelController.BeginFlash(0.05f, 0.05f, 0.02f);
        yield return new WaitForSeconds(0.1f);

        for (int i = CourseController.Instance.currentBalls.Count - 1; i >= 0; i--)
        {
            SplitBall(CourseController.Instance.currentBalls[i]);
        }

        GameManager.Instance.StartSlowMotion(1f);

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
