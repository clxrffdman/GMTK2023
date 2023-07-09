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
    public List<EventReference> musicReferences = new List<EventReference>();
    public int lastNormalLevelIndex;
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
        musicReferences.Add(FMODEventReferences.instance.BowlingAlleyMusic);
        musicReferences.Add(FMODEventReferences.instance.JapaneseMusic);
        musicReferences.Add(FMODEventReferences.instance.HalloweenMusic);
        musicReferences.Add(FMODEventReferences.instance.MainMenuMusic);
        musicReferences.Add(FMODEventReferences.instance.MainMenuMusic);
        SetCurrentCircuitFromIndex(SaveManager.Instance.circuitIndex);
        circuitMusic = AudioManager.instance.CreateEventInstance(musicReferences[SaveManager.Instance.circuitIndex]);
        circuitMusic.start();
        if (SaveManager.Instance.forceCircuitPlay && !SaveManager.Instance.firstTimePlaying)
        {
            StartCoroutine(BeginLoadedLevels());
        }

        if(SaveManager.Instance.firstTimePlaying && SaveManager.Instance.forceCircuitPlay)
        {
            PlayerController.Instance.locked = true;
            GameplayUIManager.Instance.tutorialPanel.SetActive(true);
        }

    }

    public void StartFirstPlay()
    {
        SaveManager.Instance.firstTimePlaying = false;
        PlayerController.Instance.locked = true;
        StartCoroutine(FadeFromBlack());
        //StartCoroutine(TransitionPanelController.Instance.FadeFromBlack(1f));
        //StartCoroutine(BeginLoadedLevels());
    }

    public IEnumerator FadeFromBlack() {
        PlayerController.Instance.locked = true;
        GameplayUIManager.Instance.transitionPanelController.SetBlack();
        yield return GameplayUIManager.Instance.transitionPanelController.FadeFromBlack(1f);
        StartCoroutine(BeginLoadedLevels());
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
        GameplayUIManager.Instance.bannerQuipController.RequestBannerQuip("Circuit Start!", 0.25f, 1.5f, 0.15f);
        //yield return new WaitForSeconds(1f);
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

    public void StopCircuitMusic()
    {
        circuitMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        circuitMusic.release();
    }

    public IEnumerator CircuitCompleteRoutine()
    {
        StopCircuitMusic();

        GameManager.Instance.pauseUIPanels.Push(GameplayUIManager.Instance.scorePanel);
        GameManager.Instance.TogglePause(true);
        GameplayUIManager.Instance.scoreResultUIController.SetScore(currentCircuitWinCount);
        if(SaveManager.Instance.circuitIndex + 1 > lastNormalLevelIndex)
        {
            GameplayUIManager.Instance.scoreResultUIController.DisableNextCircuitButton();
        }

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
