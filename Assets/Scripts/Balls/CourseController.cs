using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CourseController : UnitySingleton<CourseController>
{
    [Header("References")]
    [SerializeField] public Transform ballShooterTransform;
    [SerializeField] public Transform ballParentTransform;
    [SerializeField] public Collider2D randomPinArea;
    [SerializeField] public Transform playerSpawnPosition;
    [SerializeField] public List<LevelObject> circuitObjects = new List<LevelObject>() {
        new LevelObject(LevelObjectType.Lane),
        new LevelObject(LevelObjectType.Background)
    };
    [SerializeField] public GameObject pin;

    [Header("Throw List")]
    public List<BallController> currentBalls = new List<BallController>();
    public List<Thrower> currentThrowers = new List<Thrower>();
    public List<GameObject> currentPins = new List<GameObject>();

    [Header("Current Course Values")]
    public float courseWidth = 7;
    public Vector2 courseHeightBounds = new Vector2();


    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void PlaceRandomPins(int numPins, float pinPosOffset) {
        for (int i = 0; i < numPins; i++) {
            Vector2 pinPos;
            int numTries = 0;
            int maxTries = 15;
            do {
                pinPos = GlobalFunctions.RandomPointInBounds(randomPinArea.bounds);
                numTries += 1;
                if (numTries > maxTries) {
                    Debug.Log("TOO MANY ATTEMPTS AT PLACING PINS");
                    return;
                }
            } while(Physics2D.OverlapCircle(pinPos, pinPosOffset, 1 << LayerMask.NameToLayer("Pin")));
            numTries = 0;
            var newPin = Instantiate(pin, pinPos, Quaternion.identity, randomPinArea.transform);
            currentPins.Add(newPin);
        }
    }

    public IEnumerator StartLevel(Level level) {
        InitLevel(level);
        yield return level.StartLevel();
    }

    public void InitLevel(Level level) {
        //SetLevelObjects(level);
    }

    public void SetCircuitObjects(Circuit circuit) {
        foreach (LevelObject controllerObj in circuitObjects) {
            foreach (LevelObject obj in circuit.circuitObjects) {
                if (controllerObj.objectType == obj.objectType) {
                    Debug.Log("set "+obj.objectType.ToString());
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
        StartCoroutine(thrower.ThrowBall(wave.balls, wave.ApplyThrowerMod(thrower), wave.consecBallOffset));
    }

    public IEnumerator ClearInstances() {
        for (int i = currentThrowers.Count-1; i >= 0; i--) {
            Destroy(currentThrowers[i].gameObject);
        }
        currentThrowers.Clear();
        StartCoroutine(DeleteBalls(false));
        yield return DeletePins();
    }

    public IEnumerator DeletePins(bool wait=true) {
        float destroyTimer = 0.4f;
        for (int i = currentPins.Count-1; i >= 0; i--) {
            StartCoroutine(GlobalFunctions.FindComponent<HazardPin>(currentPins[i]).DeletePin(destroyTimer));
        }
        if (wait) {
            yield return new WaitForSeconds(destroyTimer);
        }
        currentPins.Clear();
    }

    public IEnumerator DeleteBalls(bool wait=true) {
        float destroyTimer = 0.4f;
        for (int i = currentBalls.Count-1; i >= 0; i--) {
            StartCoroutine(GlobalFunctions.FindComponent<BallController>(currentBalls[i].gameObject).DeleteBall(destroyTimer));
        }
        if (wait) {
            yield return new WaitForSeconds(destroyTimer);
        }
        currentBalls.Clear();
    }

    public void BallDodged(BallController ball) {
        currentBalls.Remove(ball);
        StartCoroutine(ball.DeleteBall());
    }
}
