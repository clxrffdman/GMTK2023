using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Bowling/Level")]
public class Level : ScriptableObject {
    public int levelNumber;
    public List<Wave> waves = new List<Wave>() {
        new Wave(), new Wave(), new Wave(), new Wave(), new Wave(),
        new Wave(), new Wave(), new Wave(), new Wave(), new Wave(),
    };

    public Wave currWave;

    public IEnumerator StartLevel() {
        PlayerController.Instance.locked = true;
        yield return StartWaves();
    }

    public IEnumerator EndLevel() {
        // do stuff when level ends
        Debug.Log("waves done!");
        yield return null;
    }

    public IEnumerator StartWaves() {
        // intro 
        LevelManager.Instance.currentWaveIndex = 0;
        yield return new WaitForSeconds(1);

        // go through all waves
        foreach (Wave wave in waves) {
            currWave = wave;
            yield return wave.StartWave();
            LevelManager.Instance.currentWaveIndex++;
        }
        
        // waves end
        yield return EndLevel();
    }
}