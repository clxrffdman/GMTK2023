using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Level")]
public class Level : ScriptableObject {
    public int levelNumber;
    [SerializeField]
    public List<LevelObject> levelObjects = new List<LevelObject>() {
        new LevelObject(LevelObjectType.LeftBumper),
        new LevelObject(LevelObjectType.RightBumper),
        new LevelObject(LevelObjectType.Lane),
        new LevelObject(LevelObjectType.Background)
    };
    [SerializeField]
    public List<Wave> waves = new List<Wave>() {
        new Wave(1), new Wave(2), new Wave(3), new Wave(4), new Wave(5),
        new Wave(6), new Wave(7), new Wave(8), new Wave(9), new Wave(10),
    };
    
    public void InitLevel() {
 
    }

    public IEnumerator StartLevel() {
        InitLevel();
        yield return StartWaves();
    }

    public IEnumerator EndLevel() {
        // do stuff when level ends
        Debug.Log("waves done!");
        yield return null;
    }

    public IEnumerator StartWaves() {
        // intro
        yield return new WaitForSeconds(1);

        // waves start
        foreach (Wave wave in waves) {
            yield return wave.StartWave();
        }

        // waves end
        yield return EndLevel();
    }
}