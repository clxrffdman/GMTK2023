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
        new Wave(), new Wave(), new Wave(), new Wave(), new Wave(),
        new Wave(), new Wave(), new Wave(), new Wave(), new Wave(),
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

        // go through all waves
        foreach (Wave wave in waves) {
            yield return wave.StartWave();
        }
        
        // waves end
        yield return EndLevel();
    }
}