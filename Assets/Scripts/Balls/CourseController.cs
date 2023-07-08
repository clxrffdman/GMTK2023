using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CourseController : UnitySingleton<CourseController>
{
    [Header("References")]
    [SerializeField] public Transform ballShooterTransform;
    [SerializeField] public Transform ballParentTransform;
    [SerializeField] public List<LevelObject> levelObjects = new List<LevelObject>() {
        new LevelObject(LevelObjectType.LeftBumper),
        new LevelObject(LevelObjectType.RightBumper),
        new LevelObject(LevelObjectType.Lane),
        new LevelObject(LevelObjectType.Background)
    };

    [Header("Throw List")]
    public List<BallController> currentBalls = new List<BallController>();
    public List<Thrower> currentThrowers = new List<Thrower>();

    [Header("Current Course Values")]
    public float courseWidth = 7;
    public Vector2 courseHeightBounds = new Vector2();


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

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

    public Thrower InitThrower(GameObject throwerObj) {
        Debug.Log("spawn thrower!");
        var newThrower = Instantiate(throwerObj, CourseController.Instance.ballShooterTransform);
        Thrower thrower = GlobalFunctions.FindComponent<Thrower>(newThrower);
        currentThrowers.Add(thrower);
        return thrower;
    }

    public void ThrowBalls(Thrower thrower, ThrowerWave wave) {
        StartCoroutine(thrower.ThrowBall(wave.ball, wave.ballMods));
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
}
