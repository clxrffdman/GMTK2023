using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.VFX;

public class InstantiationManager : UnitySingleton<InstantiationManager>
{

    public void SetAsChild(GameObject child)
    {
        child.transform.SetParent(transform);
    }

    public void SetAsChild(Transform child)
    {
        child.parent = transform;
    }

    public void KillAllChildren()
    {
        foreach (Transform c in transform) {
            Destroy(c.gameObject);
        }
    }

}
