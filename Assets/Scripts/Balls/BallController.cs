using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BallController : MonoBehaviour
{
    [SerializeField] private List<BallModifier> modifiers = new List<BallModifier>();
    public Rigidbody2D rb;
    public Collider2D ballCollider;
    private float activeTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        foreach(BallModifier mod in modifiers)
        {
            mod.OnSpawn(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        activeTime += Time.deltaTime;

        foreach (BallModifier mod in modifiers)
        {
            mod.OnUpdate(this, activeTime, 0);
        }

    }

    public void AddModifier(BallModifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(BallModifier mod)
    {
        modifiers.Remove(mod);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(BallModifier mod in modifiers)
        {
            mod.OnBounce(this);
        }
    }
}
