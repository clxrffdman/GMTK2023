using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BannerQuipController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI bannerText;
    public bool isQuipping = false;

    public void RequestBannerQuip(string quipText, float toDuration, float lingerDuration, float exitDuration)
    {
        if (isQuipping)
        {
            return;
        }

        StartCoroutine(QuipRoutine(quipText, toDuration, lingerDuration, exitDuration));
    }

    IEnumerator QuipRoutine(string quipText, float toDuration, float lingerDuration, float exitDuration)
    {
        isQuipping = true;
        bannerText.text = quipText;
        canvasGroup.alpha = 0;
        LeanTween.alphaCanvas(canvasGroup, 1, toDuration);

        yield return new WaitForSeconds(toDuration + lingerDuration);

        LeanTween.alphaCanvas(canvasGroup, 0, exitDuration);

        yield return new WaitForSeconds(toDuration + lingerDuration);

        isQuipping = false;
    }
}
