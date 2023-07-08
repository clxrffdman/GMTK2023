using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bowling/Circuit")]
public class Circuit : ScriptableObject
{
    public string displayName;
    public bool isEndless = false;
    public List<Level> levels;

    public int GetMaxFrames()
    {
        int rv = 0;
        foreach(Level lvl in levels)
        {
            rv += levels.Count;
        }

        return rv;
    }
}
