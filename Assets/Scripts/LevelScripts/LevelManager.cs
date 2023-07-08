using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : UnitySingleton<LevelManager>
{
    [SerializeField]
    public List<Level> levels = new List<Level>();
    public Level currentLevel;
    public int currentLevelWinCount = 0;
    public int currentWaveIndex = 0;
    public bool hasFailedCurrentWave = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartLevel() {
        GameplayUIManager.Instance.scorecardUIController.ClearElements();
        GameplayUIManager.Instance.scorecardUIController.InitScorecard(currentLevel);
        StartCoroutine(CourseController.Instance.StartLevel(currentLevel));
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
            StartLevel();
            // start level one
        }
    }

    public Level GetLevel(int levelNum) {
        foreach (Level level in levels) {
            if (levelNum == level.levelNumber) {
                return level;
            }
        }
        return null;
    }
}
