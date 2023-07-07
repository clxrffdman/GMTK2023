using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
public abstract class BallModifier : ScriptableObject
{
    public virtual void OnSpawn(BallController controller)
    {

    }

    public virtual void OnUpdate(BallController controller, float activeTime, float coursePercentage)
    {

    }

    public virtual void OnBounce(BallController controller)
    {

    }
}
