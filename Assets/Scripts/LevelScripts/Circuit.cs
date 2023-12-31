using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


[CreateAssetMenu(menuName = "Bowling/Circuit")]
public class Circuit : ScriptableObject
{
    public string displayName;
    public bool isEndless = false;
    public List<Level> levels;
    public FMODUnity.EventReference circuitMusic;
    public List<LevelObject> circuitObjects = new List<LevelObject>() {
        new LevelObject(LevelObjectType.Lane),
        new LevelObject(LevelObjectType.Background)
    };
    [ShowIf("isEndless")]
    public List<BallModifier> ballThrowMods = new List<BallModifier>();
    [ShowIf("isEndless")]
    public List<BallModifier> ballMods = new List<BallModifier>();

    public int GetMaxFrames()
    {
        int rv = 0;
        foreach(Level lvl in levels)
        {
            rv += lvl.waves.Count;
        }

        return rv;
    }
}
