using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScorecardElement : MonoBehaviour
{
    public TextMeshProUGUI indexText;
    public Image scoreImg;

    public Sprite winSprite;
    public Sprite loseSprite;

    public void Start()
    {
        scoreImg.sprite = null;
        scoreImg.color = Color.clear;
    }

    public void SetScore(bool isWin)
    {
        if (isWin)
        {
            scoreImg.sprite = winSprite;
        }
        else
        {
            scoreImg.sprite = loseSprite;
        }
    }
}
