using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
    public List<BallModifier> throwerMods;
    
    public ThrowerType throwerType;
    public bool doneThrowing = false;

    //[ToolTip("range of -90 - 90 degrees")]
    public float xOffset;
    // Start is called before the first frame update

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitBowl(ThrowerWave wave) {
        transform.position += new Vector3(wave.xOffset * CourseController.Instance.courseWidth, 0, 0);
        // something angle idk
        // spawn dudes
        ThrowBall(wave.ball, wave.ballMods);
    }

    public IEnumerator ThrowBall(GameObject ball, List<BallModifier> ballMods) {
        Debug.Log("begin throw");
        yield return new WaitForSeconds(2f); // do ball throw animation
        Debug.Log("throw ball!");
        var newBall = Instantiate(ball, CourseController.Instance.ballParentTransform);
        BallController ballController = GlobalFunctions.FindComponent<BallController>(newBall);
        ApplyThrowMods(ballController);
        ballController.InitBall(this, ballMods);
        yield return new WaitForSeconds(1f);
        doneThrowing = true;
        // pan back to player
    }

    public void ApplyThrowMods(BallController ball) {
        foreach (BallModifier mod in throwerMods) {
            ball.AddModifier(mod);
        }
    }
}
