using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;


public class BowlerProfile : ScriptableObject
{
    public string displayName;
    public Sprite portraitSprite;

    [ResizableTextArea]
    public List<string> quipLines;

}
