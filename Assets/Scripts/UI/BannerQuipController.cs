using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;
using TMPro;

public class BannerQuipController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI bannerText;
    public TypewriterByCharacter typewriter;
    public bool isQuipping = false;

    private void Start()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

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
        //typewriter.StopShowingText();
        bannerText.text = "";
        canvasGroup.alpha = 0;
        transform.localScale = new Vector3(1, 0, 1);
        LeanTween.scaleY(gameObject, 1, toDuration);
        LeanTween.alphaCanvas(canvasGroup, 1, toDuration);

        yield return new WaitForSeconds(toDuration);
        typewriter.ShowText(quipText);
        yield return new WaitForSeconds(lingerDuration);

        LeanTween.alphaCanvas(canvasGroup, 0, exitDuration);
        LeanTween.scaleY(gameObject, 0, exitDuration);

        yield return new WaitForSeconds(toDuration + lingerDuration);

        isQuipping = false;
    }
}
