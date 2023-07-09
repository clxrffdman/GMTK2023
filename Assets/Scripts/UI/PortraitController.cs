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

    [Header("Portrait Quip")]
    public TextMeshProUGUI portraitQuipText;
    public bool isQuipping;

    public BowlerProfile currentProfile;

    public void LoadProfile(BowlerProfile profile)
    {
        if(profile == null)
        {
            currentProfile = null;
            portraitSprite.sprite = null;
            portraitSprite.color = Color.clear;
            portraitText.text = "";
            return;
        }

        currentProfile = profile;
        portraitSprite.sprite = profile.portraitSprite;
        portraitSprite.color = Color.white;
        portraitText.text = profile.displayName;
        GameplayUIManager.Instance.portraitQuipController.RequestQuip("You suck at baseball!", 3f);
    }

    public void LoadProfile(Thrower profile)
    {
        if (profile == null)
        {
            currentProfile = null;
            portraitSprite.sprite = null;
            portraitSprite.color = Color.clear;
            portraitText.text = "";
            return;
        }

        portraitSprite.sprite = profile.portraitSprite;
        portraitSprite.color = Color.white;
        portraitText.text = profile.displayName;
        GameplayUIManager.Instance.portraitQuipController.RequestQuip("You suck at baseball!", 3f);
    }

    public void RequestPortraitQuip()
    {

    }
}
