using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using FMOD.Studio;

public class BallController : MonoBehaviour
{
    [Header("Current Ball Stats")]
    [SerializeField] private List<BallModifier> modifiers = new List<BallModifier>();
    private float activeTime = 0f;
    private float ignoreBallDuration = 0f;
    public float lifetime = 14;

    [Header("References")]
    public CourseController courseController;
    public Rigidbody2D rb;
    public Collider2D ballCollider;
    public SpriteRenderer ballSprite;
    public SpriteRenderer ballShadow;
    public Animator ballAnim;
    public LayerMask defaultLayerMask;
    public LayerMask ballLayerMask;

    //audio instance
    private EventInstance ballRolling;

    // Start is called before the first frame update
    void Start()
    {
        ballAnim.SetBool("thrown", true);
        Invoke("LifespanKill", lifetime);
    }

    public void LifespanKill()
    {
        if (CourseController.Instance.currentBalls.Contains(this))
        {
            CourseController.Instance.currentBalls.Remove(this);
        }

        Destroy(gameObject);
    }

    void Awake() {
        ballAnim = ballAnim != null ? ballAnim : GlobalFunctions.FindComponent<Animator>(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!ballAnim.GetBool("thrown")) {
            // have it follow the thrower
            return;
        }
        activeTime += Time.deltaTime;
        ignoreBallDuration -= ignoreBallDuration > 0 ? Time.deltaTime : 0;
        float coursePercentage = Mathf.InverseLerp(courseController.courseHeightBounds.x, courseController.courseHeightBounds.y, transform.position.y);


        for(int i = modifiers.Count-1; i >= 0; i--)
        {
            modifiers[i].OnUpdate(this, activeTime, coursePercentage);
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

        ballRolling = AudioManager.instance.CreateEventInstance(FMODEventReferences.instance.BallRolling);
        ballRolling.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        ballRolling.start();
        courseController = CourseController.Instance;
        transform.position = thrower.transform.position;
        CourseController.Instance.currentBalls.Add(this);


        foreach (BallModifier mod in ballMods) {
            BallModifier clonedMod = mod.Clone();
            modifiers.Add(clonedMod);
        }

        for (int i = modifiers.Count - 1; i >= 0; i--)
        {
            modifiers[i].OnSpawn(this);
        }
        //transform.position += new Vector3(thrower.xOffset * CourseController.Instance.courseWidth, 0, 0);

        Debug.Log("throw ball i guess");
    }

    public IEnumerator DeleteBall(float timer = 0.4f) {
        ballRolling.stop(STOP_MODE.ALLOWFADEOUT);
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.BallPit);
        GlobalFunctions.FadeOut(ballShadow, timer);
        yield return GlobalFunctions.FadeOut(ballSprite, timer);
        CourseController.Instance.currentPins.Remove(gameObject);
        Destroy(gameObject);
    }

    public void BallJump(float hgt=0.5f, float dur=0.23f, bool shake=true) {
        LeanTween.value(
            ballSprite.gameObject, (float val)=>{ballSprite.transform.localPosition = new Vector2(ballSprite.transform.localPosition.x, val);}, 0f, hgt, dur/2f
        ).setLoopPingPong(1).setEaseOutCirc().setOnComplete(()=>{
            ballSprite.transform.localPosition = new Vector2(ballSprite.transform.localPosition.x, 0f);
            if (shake) {
                CameraController.Instance.Shake(1f, 0.2f, 5f);
            }
        });
    }

    public void BallThrow() {
        Vector3 initPos = ballSprite.transform.localPosition;
        float initHght = 0.3f;
        ballSprite.transform.localPosition = new Vector3(initPos.x, initPos.y+initHght, initPos.z);
        float jumpHght = 0.2f;
        LeanTween.moveLocal(ballSprite.gameObject, new Vector3(initPos.x, ballSprite.transform.localPosition.y+jumpHght, initPos.z), 0.1f).setEaseOutCirc().setOnComplete(() => {
            LeanTween.moveLocal(ballSprite.gameObject, initPos, 0.2f).setEaseInCubic().setOnComplete(()=>{
                ballSprite.transform.localPosition = initPos;
                CameraController.Instance.Shake(1f, 0.2f, 5f);
            });
        });
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
        if(collision.gameObject.tag == "Bumper")
        {
            FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.BallBumper, ballCollider.transform.position);
        }
        if(collision.gameObject.layer == 3)
        {
            FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.BallOnBall, ballCollider.transform.position);
        }
    }
}
