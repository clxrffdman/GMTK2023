using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{

    public void MainMenu()
    {
        LevelManager.Instance.StopCircuitMusic();
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    public void RequestResume()
    {
        GameManager.Instance.TogglePause(false);
    }

    


}
