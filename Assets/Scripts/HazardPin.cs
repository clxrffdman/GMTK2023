using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class HazardPin : MonoBehaviour
{
    public SpriteRenderer sprite;
    public GameObject shadow;
    void Awake() {
        sprite = sprite != null ? sprite : GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Color spriteColor = sprite.color;
        Color prevColor = spriteColor;
        prevColor.a = 0f;
        LeanTween.value(sprite.gameObject, (Color val) => { sprite.color = val; }, prevColor, spriteColor, 0.3f);
        LeanTween.value(sprite.gameObject, (float val) => { sprite.transform.localPosition = new Vector2(0f, val); }, 5f, 0f, 0.5f);
        FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.PinPlaced, sprite.transform.position);
    }

    public IEnumerator DeletePin(float destroyTimer=0.4f) {
        //GlobalFunctions.FindComponent<Collider2D>(gameObject).enabled = false;
        yield return GlobalFunctions.FadeOut(sprite, destroyTimer);
        CourseController.Instance.currentPins.Remove(gameObject);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter2D(Collision2D collider) {
        shadow.SetActive(false);
        string collidedObject = collider.gameObject.tag;
        if (collidedObject != "Bumper" && collidedObject != "Player")
        {
            FMODUnity.RuntimeManager.PlayOneShot(FMODEventReferences.instance.BallCollision, sprite.transform.position);
        }
    }
}
