using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Thrower : MonoBehaviour
{
    //public List<BallModifier> throwerMods;
    
    public ThrowerType type;
    public string displayName;
    public Sprite portraitSprite;

    [ResizableTextArea]
    public List<string> quipLines;

    public ThrowerType throwerType;
    public bool doneThrowing = false;

    //[ToolTip("range of -90 - 90 degrees")]
    public float xOffset;
    // Start is called before the first frame update
    public List<ThrowBase> currThrowMods = new List<ThrowBase>();
    public List<BallController> currThrowerBalls = new List<BallController>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = currThrowMods.Count-1; i >= 0; i--)
        {
            currThrowMods[i].OnThrowerUpdate(this);
        }
    }

    public void InitBowl(ThrowerWave wave) {
        transform.position += new Vector3(wave.xOffset * CourseController.Instance.courseWidth, 0, 0);
        // something angle idk
        // spawn dudes
    }

    public IEnumerator ThrowBall(List<GameObject> balls, List<BallModifier> ballMods, float consecOffset=0.35f) {
        Debug.Log("begin throw");
        yield return new WaitForSeconds(1f); // do ball throw animation
        Debug.Log("throw ball!");
        for (int i = 0; i < balls.Count; i++) {
            var newBall = Instantiate(balls[i], CourseController.Instance.ballParentTransform);
            //CameraController.Instance.SetBowlerCamTarget(newBall.transform);
            BallController ballController = GlobalFunctions.FindComponent<BallController>(newBall);
            ballController.InitBall(this, ballMods);
            currThrowerBalls.Add(ballController);
            yield return new WaitForSeconds(consecOffset);
        }
        
        doneThrowing = true;
        // pan back to player
    }

    /*public void ApplyThrowMods(BallController ball) {
        foreach (BallModifier mod in throwerMods) {
            ball.AddModifier(mod);
        }
    }*/
}
