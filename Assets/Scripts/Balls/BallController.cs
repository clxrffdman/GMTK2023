using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public List<BallModifier> modifiers = new List<BallModifier>();
    private float activeTime = 0f;

    public void AddModifier()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(BallModifier mod in modifiers)
        {
            mod.OnSpawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        activeTime += Time.deltaTime;

        foreach (BallModifier mod in modifiers)
        {
            mod.OnUpdate(activeTime, 0);
        }

    }
}
