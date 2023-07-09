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
    public List<string> preThrowLines = new List<string>();

    [ResizableTextArea]
    public List<string> onThrowLines = new List<string>();

    [ResizableTextArea]
    public List<string> roundOverLoseLines = new List<string>();

    [ResizableTextArea]
    public List<string> roundOverWinLines = new List<string>();

    public ThrowerType throwerType;
    public bool doneThrowing = false;

    //[ToolTip("range of -90 - 90 degrees")]
    public float xOffset;
    // Start is called before the first frame update
    public List<ThrowBase> currThrowMods = new List<ThrowBase>();
    public List<BallController> currThrowerBalls = new List<BallController>();
    public Animator anim;
    public SpriteRenderer fakeBall;

    void Awake()
    {
        anim = anim != null ? anim : GlobalFunctions.FindComponent<Animator>(gameObject);
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
        if (wave.throwerMod is ThrowBase) {
            ((ThrowBase)wave.throwerMod).SetCustomSprite(fakeBall);
        }
        foreach (BallModifier mod in wave.ballMods) {
            mod.SetOutlineColor(fakeBall);
        }
        // something angle idk
        // spawn dudes
    }

    public IEnumerator ThrowBall(List<GameObject> balls, List<BallModifier> ballMods, float consecOffset=0.35f) {
        Debug.Log("begin throw");
        
        yield return new WaitForSeconds(1.3f); // do ball throw animation
        anim.SetBool("Run", true);
        LeanTween.move(gameObject, new Vector3(transform.position.x, transform.position.y-3.5f, transform.position.z), 1f);
        yield return new WaitForSeconds(1f);
        anim.SetBool("Bowl", true);
        yield return new WaitUntil(()=>doneThrowing);
        //yield return new WaitForSeconds(0.2f);
        Debug.Log("throw ball!");
        for (int i = 0; i < balls.Count; i++) {
            var newBall = Instantiate(balls[i], fakeBall.transform.position, Quaternion.identity, CourseController.Instance.ballParentTransform);
            fakeBall.gameObject.SetActive(false);
            //CameraController.Instance.SetBowlerCamTarget(newBall.transform);
            BallController ballController = GlobalFunctions.FindComponent<BallController>(newBall);
            ballController.BallThrow();
            ballController.InitBall(this, ballMods);
            currThrowerBalls.Add(ballController);
            yield return new WaitForSeconds(consecOffset);
        }
        
        doneThrowing = true;
        // pan back to player
    }

    public void DoneThrowing() {
        doneThrowing = true;
    }

    public void SetFakeBall(ThrowerWave wave, List<BallModifier> ballMods) {

    }

    /*public void ApplyThrowMods(BallController ball) {
        foreach (BallModifier mod in throwerMods) {
            ball.AddModifier(mod);
        }
    }*/
}
