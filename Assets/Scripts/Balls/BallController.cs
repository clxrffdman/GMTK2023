using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BallController : MonoBehaviour
{
    [Header("Current Ball Stats")]
    [SerializeField] private List<BallModifier> modifiers = new List<BallModifier>();
    private float activeTime = 0f;
    private float ignoreBallDuration = 0f;

    [Header("References")]
    public CourseController courseController;
    public Rigidbody2D rb;
    public Collider2D ballCollider;
    [SerializeField] private LayerMask defaultLayerMask;
    [SerializeField] private LayerMask ballLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        /*foreach(BallModifier mod in modifiers)
        {
            mod.OnSpawn(this);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        activeTime += Time.deltaTime;
        ignoreBallDuration -= ignoreBallDuration > 0 ? Time.deltaTime : 0;
        float coursePercentage = Mathf.InverseLerp(courseController.courseHeightBounds.x, courseController.courseHeightBounds.y, transform.position.y);

        foreach (BallModifier mod in modifiers)
        {
            mod.OnUpdate(this, activeTime, coursePercentage);
        }

        if(ignoreBallDuration > 0)
        {
            ballCollider.excludeLayers = ballLayerMask;
            rb.excludeLayers = ballLayerMask;
        }
        else
        {
            ballCollider.excludeLayers = defaultLayerMask;
            rb.excludeLayers = defaultLayerMask;
        }

    }

    public void InitBall(Thrower thrower, List<BallModifier> ballMods) {
        foreach (BallModifier mod in ballMods) {
            modifiers.Add(mod);
        }
        foreach (BallModifier mod in modifiers) {
            mod.OnSpawn(this);
        }
        courseController = CourseController.Instance;
        transform.position = thrower.transform.position;
        //transform.position += new Vector3(thrower.xOffset * CourseController.Instance.courseWidth, 0, 0);
        CourseController.Instance.currentBalls.Add(this);
        Debug.Log("throw ball i guess");
    }

    public void AddModifier(BallModifier mod)
    {
        modifiers.Add(mod);
    }

    public void RemoveModifier(BallModifier mod)
    {
        modifiers.Remove(mod);
    }

    public void SetIgnoreBallDuration(float duration)
    {
        ignoreBallDuration = duration;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        for(int i = modifiers.Count-1; i >= 0; i--)
        {
            modifiers[i].OnBounce(this);
        }

    }
}
