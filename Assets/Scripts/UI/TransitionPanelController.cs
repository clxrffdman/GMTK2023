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

    public void BeginTransition()
    {
        if (isTransitioning)
        {
            return;
        }

        isTransitioning = true;
        canvasGroup.blocksRaycasts = true;
    }
}
