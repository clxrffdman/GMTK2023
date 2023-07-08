using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public List<BallModifier> throwerMods;
    public ThrowerType throwerType;
    // should just throw one ball right? list anyways
    public BallController ballToThrow;

    //[ToolTip("range of 0-1 of where they stand when they bowl")]
    public float standPosition;
    //[ToolTip("range of -90 - 90 degrees")]
    public float xOffset;
    // Start is called before the first frame update

    void Start()
    {
        ApplyThrowMods();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator ThrowBall() {
        yield return new WaitForSeconds(2f); // do ball throw animation
        var newBall = Instantiate(ballToThrow, CourseController.Instance.ballParentTransform);
        
        newBall.GetComponent<BallController>().Throw(this);
        yield return new WaitForSeconds(1f);
        // pan back to player
    }
    public void ApplyThrowMods() {
        foreach (BallModifier mod in throwerMods) {
            ballToThrow.AddModifier(mod);
        }
    }
}
