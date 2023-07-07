using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class LevelManager : UnitySingleton<LevelManager>
{
    [SerializeField]
    public List<Level> levels = new List<Level>();
    public Level currentLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartLevel() {
        currentLevel.StartLevel();
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
