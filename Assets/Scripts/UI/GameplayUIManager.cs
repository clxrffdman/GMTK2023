using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameplayUIManager : UnitySingleton<GameplayUIManager>
{
    public PortraitController portraitController;
    public BannerQuipController bannerQuipController;
    public ScorecardUIController scorecardUIController;
    public GameObject pausePanel;
    public GameObject scorePanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            bannerQuipController.RequestBannerQuip("Spared!", 0.2f, 2f, 0.1f);
        }
    }

    public void RequestTogglePause()
    {
        GameManager.Instance.TogglePause(!GameManager.Instance.isPaused);
    }
}
