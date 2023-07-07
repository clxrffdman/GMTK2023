using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GlobalFunctions {
    public static T FindComponent<T>(GameObject obj) {
        T returnVal = obj.GetComponent<T>();
        if (!returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInChildren<T>();
        if (!returnVal.Equals(null)) {
            return returnVal;
        }
        returnVal = obj.GetComponentInParent<T>();
        if (returnVal.Equals(null)) {
            Debug.Log("ERROR: Could not Find Component");
        }
        return returnVal;
    }
}

[Serializable]
public class LevelObject {
    public LevelObjectType objectType;
    public SpriteRenderer levelObject;
    public Sprite sprite;
    public LevelObject(LevelObjectType type) {
        objectType = type;
    }
    public void SetObject(Sprite newSprite) {
        sprite = newSprite;
        levelObject.sprite = newSprite;
    }
}