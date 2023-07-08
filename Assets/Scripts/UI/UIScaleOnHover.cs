using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaleOnHover : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 baseScale = new Vector3(-1,-1,-1);
    private bool setBaseScale = false;
    [SerializeField] private bool affectParent;

    private void Start()
    {
        if(!setBaseScale)
        {
            baseScale = affectParent ? transform.parent.localScale : transform.localScale;
            setBaseScale = true;
        }
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter()
    {

        if (!affectParent)
        {
            LeanTween.scale(gameObject, 1.1f * baseScale, 0.1f).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.scale(transform.parent.gameObject, baseScale * 1.1f, 0.1f).setIgnoreTimeScale(true);
        }
    }

    public void OnPointerExit()
    {
        if (!affectParent)
        {
            LeanTween.scale(gameObject, baseScale, 0.1f).setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.scale(transform.parent.gameObject, baseScale, 0.1f).setIgnoreTimeScale(true);
        }
    }
}