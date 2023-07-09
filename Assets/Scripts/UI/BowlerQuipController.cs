using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Febucci.UI;
using TMPro;

public class BowlerQuipController : MonoBehaviour
{

    public TextMeshProUGUI quipText;
    public TypewriterByCharacter typewriter;
    public bool isQuipping;

    public void RequestQuip(string text, float lingerDuration)
    {
        if (isQuipping)
        {
            return;
        }

        StartCoroutine(QuipRoutine(text, lingerDuration));

    }

    public IEnumerator QuipRoutine(string text, float lingerDuration)
    {
        isQuipping = true;
        quipText.text = text;
        typewriter.StartShowingText();
        LeanTween.scale(gameObject, new Vector3(1.05f, 1.05f, 1.05f), 0.15f).setEaseInQuad();
        LeanTween.value(gameObject, 50, 60, 0.05f).setLoopPingPong(1).setOnUpdate(
                    (float val) =>
                    {
                        transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(50, val, 0);
                    });

        yield return new WaitForSeconds(lingerDuration);
        LeanTween.scale(gameObject, new Vector3(1f, 1f, 1f), 0.2f);
        typewriter.StartDisappearingText();
        isQuipping = false;

    }



}
