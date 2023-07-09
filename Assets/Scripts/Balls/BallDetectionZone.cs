using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class BallDetectionZone : MonoBehaviour
{
    public bool camera = false;
    [ShowIf("camera")]
    public CameraController.CameraState state;
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
        if (!camera) {
            Debug.Log("kill ball");
            CourseController.Instance.BallDodged(coll.gameObject.GetComponent<BallController>());
        }
        else {
            if (state == CameraController.CameraState.Ball && CameraController.Instance.currentCameraState == CameraController.CameraState.Player) return;
            Debug.Log("tracked "+ state.ToString());
            CameraController.Instance.SetCameraState(state, coll.gameObject.GetComponent<BallController>());
        }
    }
}
