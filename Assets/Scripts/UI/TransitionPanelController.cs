using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionPanelController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
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

        StartCoroutine(TransitionRoutine(inDuration, lingerDuration, exitDuration));
    }

    public IEnumerator TransitionRoutine(float inDuration, float lingerDuration, float exitDuration)
    {
        isTransitioning = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;

        LeanTween.alphaCanvas(canvasGroup, 1, inDuration);

        yield return new WaitForSeconds(inDuration + lingerDuration);

        LeanTween.alphaCanvas(canvasGroup, 0, exitDuration);

        yield return new WaitForSeconds(exitDuration);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;

    }
}
