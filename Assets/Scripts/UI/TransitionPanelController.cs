using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPanelController : MonoBehaviour {
    public CanvasGroup canvasGroup;
    public Image backdrop;
    public bool isFlashing = false;
    public bool isTransitioning = false;

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public void BeginTransition(float inDuration, float lingerDuration, float exitDuration)
    {
        if (isTransitioning)
        {
            return;
        }

        gameObject.SetActive(true);
        StartCoroutine(TransitionRoutine(inDuration, lingerDuration, exitDuration));
    }

    public void BeginFlash(float inDuration, float lingerDuration, float exitDuration)
    {
        if (isFlashing)
        {
            return;
        }

        StartCoroutine(FlashRoutine(inDuration, lingerDuration, exitDuration));
    }

    public IEnumerator TransitionRoutine(float inDuration, float lingerDuration, float exitDuration)
    {
        isTransitioning = true;
        backdrop.color = Color.black;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;

        LeanTween.alphaCanvas(canvasGroup, .95f, inDuration);

        yield return new WaitForSeconds(inDuration + lingerDuration);

        LeanTween.alphaCanvas(canvasGroup, 0, exitDuration);

        yield return new WaitForSeconds(exitDuration);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        isTransitioning = false;
        gameObject.SetActive(false);

    }

    public void SetBlack() {
        backdrop.color = Color.black;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        Debug.Log("set to black pleeeeeeease man");
    }

    public IEnumerator FadeToBlack(float dur, bool fadeBack=true) {
        isTransitioning = true;
        backdrop.color = Color.black;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;

        LeanTween.alphaCanvas(canvasGroup, 1f, dur);
        yield return new WaitForSeconds(dur);
        if (fadeBack) {
            yield return FadeFromBlack(0.5f);
        }
    }

    public IEnumerator FadeFromBlack(float dur) {
        LeanTween.alphaCanvas(canvasGroup, 0, dur);
        yield return new WaitForSeconds(dur);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
        isTransitioning = false;
        gameObject.SetActive(false);
    }

    

    public IEnumerator FlashRoutine(float inDuration, float lingerDuration, float exitDuration)
    {
        backdrop.color = Color.black;
        isFlashing = true;
        LeanTween.value(gameObject, 0, 1, inDuration).setOnUpdate(
                    (float val) =>
                    {
                        SetTransitionColor(val);
                    }
                );
        yield return new WaitForSeconds(inDuration + lingerDuration);
        LeanTween.value(gameObject, 1, 0, inDuration).setOnUpdate(
                    (float val) =>
                    {
                        SetTransitionColor(val);
                    }
                );
        yield return new WaitForSeconds(exitDuration);

        isFlashing = false;

    }

    public void SetTransitionColor(float val)
    {
        backdrop.color = new Color(val, val, val, 1);
    }
}
