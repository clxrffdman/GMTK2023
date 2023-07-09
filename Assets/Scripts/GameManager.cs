using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : UnitySingleton<GameManager>
{
    [Header("Current Game State")]
    public bool isPaused = false;
    public bool hasLost = false;
    public bool canPause = true;
    public float baseSlowMoDuration = 0;
    public float slowMoDuration = 0;

    public SpriteRenderer bgSr;
    public List<Sprite> bgList = new List<Sprite>();

    public Stack<GameObject> pauseUIPanels = new Stack<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        bgSr.sprite = bgList[SaveManager.Instance.circuitIndex];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlowMotion();   

    }

    public void StartSlowMotion(float duration) {
        slowMoDuration = duration;
        baseSlowMoDuration = slowMoDuration;
    }

    public void UpdateSlowMotion() {
        if (slowMoDuration <= 0) return;
        slowMoDuration = Mathf.Max(slowMoDuration-Time.unscaledDeltaTime, 0f);
        Time.timeScale = Mathf.Max(1f-(slowMoDuration/baseSlowMoDuration), 0.2f);
        Time.fixedDeltaTime = 0.01666667f * Time.timeScale;

        if (Time.timeScale == 1)
        {
            Time.fixedDeltaTime = 0.01666667f;
        } 
    }

    public void TogglePanel(GameObject panel, bool state)
    {
        panel.SetActive(state);
    }

    public void TogglePause(bool shouldPause)
    {
        if (hasLost || !canPause)
        {
            return;
        }

        if (shouldPause)
        {

            isPaused = true;

            Time.timeScale = 0;

            //Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;


            if (pauseUIPanels.Count == 0)
            {
                pauseUIPanels.Push(GameplayUIManager.Instance.pausePanel);
                TogglePanel(GameplayUIManager.Instance.pausePanel, true);
            }
            else
            {

                TogglePanel(pauseUIPanels.Peek(), true);
            }

            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isPaused", 1.0f);
            //ambianceEventInstance.setPaused(true);
        }
        else
        {

            if (pauseUIPanels.Count > 0)
            {
                TogglePanel(pauseUIPanels.Pop(), false);
            }

            if (pauseUIPanels.Count >= 1)
            {
                Debug.Log("Panels: " + pauseUIPanels.Peek().name);
                return;
            }

            isPaused = false;
            Time.timeScale = 1;

            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("isPaused", 0.0f);
        }

    }


}
