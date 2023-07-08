using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameplayUIManager : UnitySingleton<GameplayUIManager>
{
    public PortraitController portraitController;
    public BannerQuipController bannerQuipController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            bannerQuipController.RequestBannerQuip("Spared!", 0.2f, 2f, 0.1f);
        }
    }
}
