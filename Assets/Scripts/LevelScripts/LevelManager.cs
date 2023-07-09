using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;

public class LevelManager : UnitySingleton<LevelManager>
{

    [Header("Pre Set Information")]
    public List<Circuit> circuits = new List<Circuit>();

    public FMODUnity.EventReference currentCircuitMusic;
    public EventInstance circuitMusic;

    [Header("Current Circuit Information")]
    public CourseState currentCourseState;
    public Circuit currentCircuit;
    public int currentCircuitWinCount = 0;
    public List<Level> currentCircuitLevels = new List<Level>();
    public Level currentLevel;
    public int currentLevelWinCount = 0;
    public int currentWaveIndex = 0;
    public bool hasFailedCurrentWave = false;


    // Start is called before the first frame update
    void Start()
    {
        circuitMusic = AudioManager.instance.CreateEventInstance(currentCircuitMusic);
        circuitMusic.start();
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
        CourseController.Instance.SetCircuitObjects(currentCircuit);
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
        
        yield return new WaitForSeconds(1f);
        for(int i = 0; i < currentCircuitLevels.Count; i++)
        {
            currentLevelWinCount = 0;
            currentLevel = currentCircuitLevels[i];
            if (!currentLevel)
            {
                Debug.Log("level " + i + " not found");
            }
            else
            {
                yield return StartLevel();
            }


            yield return new WaitForSeconds(2f);

            if (!CheckLevelSuccess())
            {
                currentCircuitWinCount -= currentLevelWinCount;
                i--;
            }

        }

        StartCoroutine(CircuitCompleteRoutine());

    }

    public void RestartCircuit()
    {
        SaveManager.Instance.forceCircuitPlay = true;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public bool CheckLevelSuccess()
    {
        if(currentLevelWinCount >= 3)
        {
            return true;
        }

        return false;
    }

    public IEnumerator CircuitCompleteRoutine()
    {
        GameManager.Instance.pauseUIPanels.Push(GameplayUIManager.Instance.scorePanel);
        GameManager.Instance.TogglePause(true);
        GameplayUIManager.Instance.scoreResultUIController.SetScore(currentCircuitWinCount);
        if (SaveManager.Instance.scoreDictionary.ContainsKey(SaveManager.Instance.circuitIndex))
        {
            SaveManager.Instance.scoreDictionary[SaveManager.Instance.circuitIndex] = 
                Mathf.Max(SaveManager.Instance.scoreDictionary[SaveManager.Instance.circuitIndex], currentCircuitWinCount);
        }
        else
        {
            SaveManager.Instance.scoreDictionary[SaveManager.Instance.circuitIndex] = currentCircuitWinCount;
        }
        
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
