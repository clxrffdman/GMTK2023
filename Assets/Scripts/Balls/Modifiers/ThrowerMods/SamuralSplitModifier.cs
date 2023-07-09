using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SamuralSplitModifier : ThrowBase
{
    public int splitCount = 2;
    public float projectileSpread = 50;
    public float spreadForce = 30;

    public bool hasStarted = false;
    public bool hasSplit = false;
    public float currentCoursePercent = 0;
    public float coursePercentDark = 0.5f;
    public float coursePercentSplit = 0.7f;
    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
    }

    public override void OnThrowerUpdate(Thrower thrower)
    {
        if (!thrower.doneThrowing) return;
        base.OnThrowerUpdate(thrower);
        
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        currentCoursePercent = coursePercentage;
        if (coursePercentage >= coursePercentDark && !hasStarted)
        {
            hasStarted = true;
            controller.StartCoroutine(SamuraiSplit(controller));
        }

        if (coursePercentage >= coursePercentSplit && !hasSplit && hasStarted)
        {
            hasSplit = true;
        }
    }

    public IEnumerator SamuraiSplit(BallController controller)
    {
        controller.StartCoroutine(GameplayUIManager.Instance.transitionPanelController.FadeToBlack(0.35f, 0, false));
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.SamuraiSlowdown);
        yield return new WaitUntil(() => (currentCoursePercent >= coursePercentSplit));
        hasSplit = true;
        GameplayUIManager.Instance.transitionPanelController.BeginFlash(0.05f, 0.05f, 0.02f);
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.SamuraiSlash);
        yield return new WaitForSeconds(0.12f);
        controller.StartCoroutine(GameplayUIManager.Instance.transitionPanelController.FadeFromBlack(0.1f));
        yield return new WaitForSeconds(0.1f);

        for (int i = CourseController.Instance.currentBalls.Count - 1; i >= 0; i--)
        {
            SplitBall(CourseController.Instance.currentBalls[i]);
        }

        GameManager.Instance.StartSlowMotion(1f);

        controller.RemoveModifier(this);

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
        BallController ballCont = bullet.gameObject.GetComponent<BallController>();
        ballCont.RemoveModifier(this);
        ballCont.rb.velocity = Vector2.zero;
        ballCont.rb.AddForce(dir * spreadForce, ForceMode2D.Impulse);
        ballCont.SetIgnoreBallDuration(0.2f);
        CourseController.Instance.currentBalls.Add(ballCont);

    }
}
