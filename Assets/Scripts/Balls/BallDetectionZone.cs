using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDetectionZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.layer != 3) return;
        Debug.Log("kill ball");
        CourseController.Instance.BallDodged(GlobalFunctions.FindComponent<BallController>(coll.gameObject));
    }
}
