using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using NaughtyAttributes;

[System.Serializable]
public class ThrowProfile
{
    [MinValue(-1), MaxValue(1)]
    public float xOffset = 0;
    public List<BallModifier> modifiers = new List<BallModifier>() {};
}
