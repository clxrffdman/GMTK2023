using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CourseController : UnitySingleton<CourseController>
{
    [Header("References")]
    [SerializeField] public Transform ballShooterTransform;
    [SerializeField] public Transform ballParentTransform;
    public GameObject baseBallReference;

    [Header("Throw List")]
    public List<ThrowProfile> throwProfiles = new List<ThrowProfile>();
    
    [SerializeField]
    public List<LevelObject> levelObjects = new List<LevelObject>() {
        new LevelObject(LevelObjectType.LeftBumper),
        new LevelObject(LevelObjectType.RightBumper),
        new LevelObject(LevelObjectType.Lane),
        new LevelObject(LevelObjectType.Background)
    };

    public List<BallController> currentBalls = new List<BallController>();
    public List<Thrower> currentThrowers = new List<Thrower>();

    [Header("Current Course Values")]
    public int currentThrowIndex = 0;
    public float courseWidth = 7;
    public Vector2 courseHeightBounds = new Vector2();


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (baseBallReference == null)
            {
                return;
            }

            if (currentThrowIndex >= throwProfiles.Count) {
                return;
            }

            var newBall = Instantiate(baseBallReference, ballParentTransform);
            newBall.transform.position = ballShooterTransform.position;
            newBall.GetComponent<BallController>().courseController = this;

            ThrowProfile profile = throwProfiles[currentThrowIndex];
            newBall.transform.position += new Vector3(profile.xOffset * courseWidth, 0, 0);
            foreach (BallModifier mod in profile.modifiers)
            {
                newBall.GetComponent<BallController>().AddModifier(mod);
            }

            currentThrowIndex++;
            

        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentThrowIndex = 0;
        }
    }

    public IEnumerator StartLevel(Level level) {
        InitLevel(level);
        yield return level.StartLevel();
    }

    public void InitLevel(Level level) {
        SetLevelObjects(level);
    }

    public void SetLevelObjects(Level level) {
        foreach (LevelObject controllerObj in levelObjects) {
            foreach (LevelObject obj in level.levelObjects) {
                if (controllerObj.objectType == obj.objectType) {
                    controllerObj.SetObject(obj.sprite);
                }
            }
        }
    }

    public void ClearThrowers() {
        for (int i = currentThrowers.Count-1; i >= 0; i--) {
            Destroy(currentThrowers[i].gameObject);
        }
        currentBalls.Clear();
        currentThrowers.Clear();
    }

    public void BallDodged(BallController ball) {
        currentBalls.Remove(ball);
        Destroy(ball.gameObject);
    }

    public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.layer != LayerMask.NameToLayer("Ball")) return;
        BallDodged(GlobalFunctions.FindComponent<BallController>(coll.gameObject));
    }
}
