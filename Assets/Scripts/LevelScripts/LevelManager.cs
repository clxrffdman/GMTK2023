using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : UnitySingleton<LevelManager>
{

    [Header("Pre Set Information")]
    [SerializeField]
    public List<Circuit> circuits = new List<Circuit>();

    [Header("Current Circuit Information")]
    [SerializeField]
    public Circuit currentCircuit;
    public List<Level> currentCircuitLevels = new List<Level>();
    public Level currentLevel;
    public int currentLevelWinCount = 0;
    public int currentWaveIndex = 0;
    public bool hasFailedCurrentWave = false;


    // Start is called before the first frame update
    void Start()
    {
        SetCurrentCircuitFromIndex(SaveManager.Instance.circuitIndex);
        if (SaveManager.Instance.forceCircuitPlay)
        {
            StartCoroutine(BeginLoadedLevels());
        }
    }

    public void SetCurrentCircuitFromIndex(int index)
    {
        if(index < 0 || index > circuits.Count)
        {
            Debug.Log("circuit " + index + " is an invalid circuit (too small or too large index)");
            return;
        }

        currentCircuit = circuits[index];
        currentCircuitLevels = currentCircuit.levels;
    }

    public IEnumerator StartLevel() {
        GameplayUIManager.Instance.scorecardUIController.ClearElements();
        GameplayUIManager.Instance.scorecardUIController.InitScorecard(currentLevel);
        yield return CourseController.Instance.StartLevel(currentLevel);
    }

    public void EndLevel() {
        Debug.Log("level finished!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentLevel = GetLevel(1);
            if (!currentLevel) {
                Debug.Log("level 1 not found");
                return;
            }
            currentLevelWinCount = 0;
            StartCoroutine(StartLevel());
            // start level one
        }
    }

    public IEnumerator BeginLoadedLevels()
    {
        currentLevelWinCount = 0;
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < currentCircuitLevels.Count; i++)
        {
            currentLevel = currentCircuitLevels[i];
            if (!currentLevel)
            {
                Debug.Log("level " + i + " not found");
            }
            else
            {
                yield return StartLevel();
            }


            yield return new WaitForSeconds(4f);

        }

        StartCoroutine(CircuitCompleteRoutine());

    }

    public IEnumerator CircuitCompleteRoutine()
    {
        yield return null;
    }

    public Level GetLevel(int levelNum) {
        foreach (Level level in currentCircuitLevels) {
            if (levelNum == level.levelNumber) {
                return level;
            }
        }
        return null;
    }
}
