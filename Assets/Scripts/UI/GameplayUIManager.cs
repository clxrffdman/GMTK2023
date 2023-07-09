using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameplayUIManager : UnitySingleton<GameplayUIManager>
{
    public PortraitController portraitController;
    public BowlerQuipController portraitQuipController;
    public BannerQuipController bannerQuipController;
    public ScorecardUIController scorecardUIController;
    public ScoreResultUIController scoreResultUIController;
    public TransitionPanelController transitionPanelController;
    public GameObject pausePanel;
    public GameObject scorePanel;
    public GameObject tutorialPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            bannerQuipController.RequestBannerQuip("Spared!", 0.2f, 2f, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            transitionPanelController.BeginTransition(0.3f, 2, 0.2f);
        }
    }

    public void RequestTogglePause()
    {
        GameManager.Instance.TogglePause(!GameManager.Instance.isPaused);
    }

    public void RequestExitTutorial()
    {
        tutorialPanel.SetActive(false);
        LevelManager.Instance.StartFirstPlay();
    }
}
