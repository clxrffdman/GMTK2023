using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GlobalFunctions {
    public static Vector3 RandomPointInBounds(Bounds bounds) {
        return new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );
    }
    public static IEnumerator FadeOut(SpriteRenderer sprite, float timer = 0.4f) {
        Color spriteColor = sprite.color;
        Color newColor = spriteColor;
        newColor.a = 0f;
        Debug.Log("fading!");
        LeanTween.value(sprite.gameObject, (Color val) => { sprite.color = val; }, spriteColor, newColor, timer);
        yield return new WaitForSeconds(timer);
    }

    public static IEnumerator FadeIn(SpriteRenderer sprite, float timer = 0.4f)
    {
        Color spriteColor = sprite.color;
        spriteColor.a = 0;
        Color newColor = spriteColor;
        newColor.a = 1;
        LeanTween.value(sprite.gameObject, (Color val) => { sprite.color = val; }, spriteColor, newColor, timer);
        yield return new WaitForSeconds(timer);
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