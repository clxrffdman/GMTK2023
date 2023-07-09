using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PortraitController : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI portraitText;
    public Image portraitSprite;

    [Header("Ball Info")]
    public TextMeshProUGUI ballInfoText;
    public Image ballImg;
    public bool isQuipping;

    public Thrower currentThrower = null;
    public void LoadProfile(BowlerProfile profile)
    {
        if(profile == null)
        {
            portraitSprite.sprite = null;
            portraitSprite.color = Color.clear;
            portraitText.text = "";
            return;
        }

        portraitSprite.sprite = profile.portraitSprite;
        portraitSprite.color = Color.white;
        portraitText.text = profile.displayName;
    }

    public void LoadProfile(Thrower profile)
    {
        if (profile == null)
        {
            currentThrower = profile;
            portraitSprite.sprite = null;
            portraitSprite.color = Color.clear;
            portraitText.text = "";
            return;
        }

        currentThrower = profile;
        portraitSprite.sprite = profile.portraitSprite;
        portraitSprite.color = Color.white;
        portraitText.text = profile.displayName;
    }

    public void LoadBallType(BallModifier throwType)
    {
        if(throwType == null || !(throwType is ThrowBase))
        {
            return;
        }

        if(((ThrowBase)throwType).useCustomSprite && ((ThrowBase)throwType).customSprite != null)
        {
            ballImg.sprite = ((ThrowBase)throwType).customSprite;
            ballImg.color = Color.white;
            ballInfoText.text = throwType.description;
        }
        else
        {
            ballImg.sprite = null;
            ballImg.color = Color.clear;
            ballInfoText.text = "";
        }
    }

    public void RequestPortraitQuip()
    {
        string selectedQuip = "";
        if(currentThrower == null)
        {
            GameplayUIManager.Instance.portraitQuipController.RequestQuip("Invalid Quip", 1);
            return;
        }
        switch (LevelManager.Instance.currentCourseState)
        {
            case CourseState.PreppingThrow:
                if(currentThrower.preThrowLines.Count > 0)
                {
                    selectedQuip = currentThrower.preThrowLines[Random.Range(0, currentThrower.preThrowLines.Count)];
                }
                break;
            case CourseState.Throwing:
                if (currentThrower.onThrowLines.Count > 0)
                {
                    selectedQuip = currentThrower.onThrowLines[Random.Range(0, currentThrower.onThrowLines.Count)];
                }
                break;
            case CourseState.RoundEndSuccess:
                if (currentThrower.roundOverWinLines.Count > 0)
                {
                    selectedQuip = currentThrower.roundOverWinLines[Random.Range(0, currentThrower.roundOverWinLines.Count)];
                }
                break;
            case CourseState.RoundEndFail:
                if (currentThrower.roundOverLoseLines.Count > 0)
                {
                    selectedQuip = currentThrower.roundOverLoseLines[Random.Range(0, currentThrower.roundOverLoseLines.Count)];
                }
                break;
        }

        GameplayUIManager.Instance.portraitQuipController.RequestQuip(selectedQuip, 1f);
    }
}
