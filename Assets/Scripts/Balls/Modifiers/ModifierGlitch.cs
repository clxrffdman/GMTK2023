using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierGlitch : BallModifier
{
    public float minTeleportDistance;
    public float maxTeleportDistance;
    public float teleportInterval;

    private float counter = 0;
    public LayerMask teleportCheckLayerMask;
    private bool isGlitching = false;

    public GameObject teleportIndicatorPrefab;
    public GameObject teleportIndicator = null;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        counter = teleportInterval;
        teleportIndicator = (teleportIndicator == null) ? GameObject.Instantiate(teleportIndicatorPrefab, controller.transform) : teleportIndicator;
        GlobalFunctions.FadeOut(teleportIndicator.GetComponent<SpriteRenderer>(), 0.2f);
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if (counter >= 0)
        {
            counter -= Time.deltaTime;
        }
        else if(!isGlitching)
        {
            isGlitching = true;
            controller.StartCoroutine(GlitchTeleport(controller));
        }

    }

    public IEnumerator GlitchTeleport(BallController controller)
    {
        Vector3 newPosOffset = Vector3.zero;
        teleportIndicator.transform.localPosition = Vector3.zero;
        Debug.Log("checking glitch validity");

        bool leftValid = false;
        bool rightValid = false;

        float newXPos = 0;

        RaycastHit2D leftHit = Physics2D.Raycast(controller.transform.position, -Vector3.right, Mathf.Infinity, teleportCheckLayerMask);
        RaycastHit2D rightHit = Physics2D.Raycast(controller.transform.position, Vector3.right, Mathf.Infinity, teleportCheckLayerMask);

        if (leftHit.distance > minTeleportDistance)
        {
            leftValid = true;
        }

        if (rightHit.distance > minTeleportDistance)
        {
            rightValid = true;
        }

        if (leftValid && rightValid)
        {
            if (Random.Range(0, 2) == 0)
            {
                newXPos = Random.Range(-minTeleportDistance, -leftHit.distance);
            }
            else
            {
                newXPos = Random.Range(minTeleportDistance, rightHit.distance);
            }
        }
        else if (leftValid)
        {
            newXPos = Random.Range(-minTeleportDistance, -leftHit.distance);
        }
        else if (rightValid)
        {
            newXPos = Random.Range(minTeleportDistance, rightHit.distance);
        }
        else
        {
            isGlitching = false;
            counter = teleportInterval;
            yield break;
        }

        newPosOffset = new Vector3(newXPos, 0, 0);

        teleportIndicator.transform.localPosition += newPosOffset;

        yield return GlobalFunctions.FadeIn(teleportIndicator.GetComponent<SpriteRenderer>(), 0.2f);

        controller.transform.localPosition += newPosOffset;
        teleportIndicator.GetComponent<SpriteRenderer>().color = teleportIndicator.GetComponent<SpriteRenderer>().color * new Color(1, 1, 1, 0);

        counter = teleportInterval;

        isGlitching = false;
    }
}
