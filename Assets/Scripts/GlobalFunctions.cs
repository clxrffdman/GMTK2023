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
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
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
        if (newSprite == null) return;
        sprite = newSprite;
        levelObject.sprite = newSprite;
    }
}