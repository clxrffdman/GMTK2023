using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Bowling/Circuit")]
public class Circuit : ScriptableObject
{
    public string displayName;
    public bool isEndless = false;
    public List<Level> levels;
}
