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

    public GameObject teleportIndicator;

    public override void OnSpawn(BallController controller)
    {
        base.OnSpawn(controller);
        counter = teleportInterval;
    }

    public override void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {
        base.OnUpdate(controller, activeTime, coursePercentage);

        if (counter >= 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            bool leftValid = false;
            bool rightValid = false;

            float newXPos = 0;

            RaycastHit2D leftHit = Physics2D.Raycast(controller.transform.position, -Vector3.right, teleportCheckLayerMask);
            RaycastHit2D rightHit = Physics2D.Raycast(controller.transform.position, Vector3.right, teleportCheckLayerMask);

            if(leftHit.distance > minTeleportDistance)
            {
                leftValid = true;
            }

            if (rightHit.distance > minTeleportDistance)
            {
                rightValid = true;
            }

            if (leftValid && rightValid)
            {
                if(Random.Range(0, 2) == 0)
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

            controller.transform.localPosition += new Vector3(newXPos, 0, 0);



            counter = teleportInterval;
        }

    }
}
